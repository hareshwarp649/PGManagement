using AutoMapper;
using bca.api.DTOs;
using bca.api.Enums;
using bca.api.Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Enums;

namespace bca.api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly IStateService _stateService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        //private readonly ITeamService _teamService;
        //private readonly IDepartmentService _departmentService;
        private readonly IUserRoleService _userRoleService;

        public EmployeeService(IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context, IRoleService roleService, IUserRoleService userRoleService)
        {
            _employeeRepository = employeeRepository;
            //_stateService = stateService;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _roleService = roleService;
            _userRoleService = userRoleService;
            //_teamService = teamService;
            //_departmentService = departmentService;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync(x => !x.IsDeleted, 
                q => q.Include(u => u.Manager));
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        //public async Task<IEnumerable<object>> GetAllEmployeesByStateIdAsync(int stateId)
        //{
        //    var employees = await _employeeRepository.GetAllAsync(x => x.StateId == stateId && !x.IsDeleted);
        //    return employees.Select(x => new{ x.Id, Name = (x.FirstName + " " + x.LastName).Trim()});
        //}

        public async Task<IEnumerable<EmployeeDTO>> GetAllReportingEmployeesAsync(int managerId)
        {
            var employees = await _employeeRepository.GetAllAsync(x => x.ManagerId == managerId && !x.IsDeleted,
                q => q.Include(u => u.Manager)
                       );
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, x => x.Manager!, x => x.Role!);
            return employee == null ? null : _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO employeeDto, bool updateIfExists = false)
        {
            var emp = await _employeeRepository.GetByIdAsync(employeeDto.Id);
            if (emp != null && !updateIfExists)
                throw new Exception("Employee with same ID already exists");

            if (emp != null && updateIfExists)
            {
                if (employeeDto.ManagerId.HasValue && emp.ManagerId != employeeDto.ManagerId)
                {
                    emp.ManagerId = employeeDto.ManagerId;
                    var updatedEmployee = await _employeeRepository.UpdateAsync(emp);
                    return _mapper.Map<EmployeeDTO>(updatedEmployee);
                }
                return _mapper.Map<EmployeeDTO>(emp);
            }

            // Start a transaction (if using EF Core)
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Employee
                    var employee = _mapper.Map<Employee>(employeeDto);
                    var createdEmployee = await _employeeRepository.AddAsync(employee);

                    if (createdEmployee != null)
                    {
                        var user = await _userManager.FindByNameAsync("" + createdEmployee.Id);
                        if (user != null && !user.IsDeleted)
                        {
                            // User with same username already exists
                            throw new Exception("Username already exists.");
                        }

                        if(user != null && user.IsDeleted)
                        {
                            await _userRoleService.DeleteRolesAsync(user.Id);
                            // Delete the existing user
                            await _userManager.DeleteAsync(user);
                        }

                        // Create User for Employee
                        user = new ApplicationUser
                        {
                            UserName = "" + createdEmployee.Id,
                            Email = createdEmployee.OfficialEmail,
                            PhoneNumber = createdEmployee.PersonalPhoneNumber,
                            EntityId = createdEmployee.Id,
                            UserType = UserType.Employee
                        };
                        var result = await _userManager.CreateAsync(user, "TestPassword@1234");

                        if (!result.Succeeded)
                        {
                            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                        }

                        // Assign Role
                        await _userRoleService.AddRolesAsync(user.Id, new List<int> { employeeDto!.RoleId!.Value });

                        // Commit transaction
                        await transaction.CommitAsync();
                    }

                    return _mapper.Map<EmployeeDTO>(createdEmployee);
                }
                catch (Exception ex)
                {
                    // Rollback on failure
                    await transaction.RollbackAsync();
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public async Task<EmployeeDTO?> UpdateEmployeeAsync(int id, UpdateEmployeeDTO employeeDto)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null) return null;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update Role
                if (employeeDto.RoleId != existingEmployee.RoleId)
                {
                    var user = await _userManager.FindByNameAsync("" + existingEmployee.Id);
                    if (user != null)
                    {
                        // Remove existing Role
                        await _userRoleService.RemoveRolesAsync(user.Id, [existingEmployee.RoleId!.Value]);

                        // Assign new Role
                        await _userRoleService.AddRolesAsync(user.Id, [employeeDto!.RoleId!.Value]);
                    }
                }
                _mapper.Map(employeeDto, existingEmployee);
                var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);
                // Commit transaction
                await transaction.CommitAsync();

                return _mapper.Map<EmployeeDTO>(updatedEmployee);
            }
            catch (Exception ex)
            {
                // Rollback on failure
                await transaction.RollbackAsync();
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<bool> ActivateDeactivateEmployeeAsync(int id, string action)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null) return false;

            existingEmployee.IsActive = action == "activate";
            var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id, bool softDelete = true)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    bool result = false;
                    if (softDelete)
                    {
                        var employee = await _employeeRepository.GetByIdAsync(id);
                        if (employee == null) return false;
                        employee.IsDeleted = true;
                        await _employeeRepository.UpdateAsync(employee);

                        var user = await _userManager.FindByNameAsync("" + employee.Id);
                        if (user != null && !user.IsDeleted)
                        {
                            user.IsDeleted = true;
                            await _userManager.UpdateAsync(user);
                        }
                        result = true;
                    }
                    else
                    {
                        result = await _employeeRepository.DeleteAsync(id);
                    }
                    // Commit transaction
                    if (result)
                        await transaction.CommitAsync();
                    else
                        await transaction.RollbackAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    // Rollback on failure
                    await transaction.RollbackAsync();
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public async Task<(IEnumerable<EmployeeDTO>, int)> SearchEmployeesAsync(string? name, int? roleId, int? departmentId, int? teamId, int? stateId,
            string? sortBy, string? sortOrder, int pageNumber, int pageSize)
        {
            var (employees, totalRecords) = await _employeeRepository.SearchEmployeesAsync(name, roleId, departmentId, teamId, stateId, sortBy, sortOrder, pageNumber, pageSize);            
            return (_mapper.Map<IEnumerable<EmployeeDTO>>(employees), totalRecords);
        }

        //public async Task<bool> BulkInsertFromExcelAsync(IFormFile file)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;  // ✅ Set EPPlus license context

        //    if (file == null || file.Length == 0)
        //        return false;

        //    using (var stream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(stream);
        //        using (var package = new ExcelPackage(stream))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0];
        //            int rowCount = worksheet.Dimension.Rows;

        //            for (int row = 2; row <= rowCount; row++) // Start from row 2 (Skip header)
        //            {
        //                var state = worksheet.Cells[row, 3].Value?.ToString() ?? "";
        //                State? stateEntity = null;
        //                if (!string.IsNullOrWhiteSpace(state))
        //                {
        //                    stateEntity = await _stateService.GetStateByNameAsync(state);
        //                    if (stateEntity == null)
        //                    {
        //                        throw new Exception($"State Not found {state} For Row number {row - 1}");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception($"State is required For Row number {row - 1}");
        //                }

        //                var team = worksheet.Cells[row, 2].Value?.ToString() ?? "";
        //                Team? teamEntity = null;
        //                if (!string.IsNullOrWhiteSpace(team))
        //                {
        //                    teamEntity = await _teamService.GetTeamByNameAsync(team);
        //                    if (teamEntity == null)
        //                    {
        //                        throw new Exception($"Team Not found {team} For Row number {row - 1}");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception($"Team is required For Row number {row - 1}");
        //                }

        //                var department = worksheet.Cells[row, 23].Value?.ToString() ?? "";
        //                Department? departmentEntity = null;
        //                if (!string.IsNullOrWhiteSpace(department))
        //                {
        //                    departmentEntity = await _departmentService.GetDepartmentByNameAsync(department);
        //                    if (departmentEntity == null)
        //                    {
        //                        throw new Exception($"Department Not found {department} For Row number {row - 1}");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception($"Department is required For Row number {row - 1}");
        //                }

        //                var role = worksheet.Cells[row, 22].Value?.ToString() ?? "";
        //                Role? roleEntity = null;
        //                if (!string.IsNullOrWhiteSpace(role))
        //                {
        //                    roleEntity = await _roleService.GetRoleByNameAsync(role);
        //                    if (roleEntity == null)
        //                    {
        //                        throw new Exception($"Role Not found {role} For Row number {row - 1}");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception($"Role is required For Row number {row - 1}");
        //                }

        //                if (!DateTime.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out DateTime dobDate))
        //                {
        //                    throw new Exception($"Invalid Date of Birth For Row number {row - 1}");
        //                }

        //                if (!Enum.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out Gender gender))
        //                {
        //                    if (worksheet.Cells[row, 8].Value?.ToString() == "F")
        //                        gender = Gender.Female;
        //                    else
        //                        gender = Gender.Male;
        //                }

        //                string strEmployeeId = worksheet.Cells[row, 1].Value?.ToString() ?? "";
        //                int employeeId = 0;
        //                if (!string.IsNullOrWhiteSpace(strEmployeeId))
        //                {
        //                    if (!int.TryParse(strEmployeeId, out employeeId))
        //                    {
        //                        throw new Exception($"Invalid Employee Id {strEmployeeId} For Row number {row - 1}");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception($"Employee Id is required For Row number {row - 1}");
        //                }

        //                string strManagerId = worksheet.Cells[row, 20].Value?.ToString() ?? "";
        //                int managerId = 0;
        //                if (!string.IsNullOrWhiteSpace(strManagerId))
        //                {
        //                    if (int.TryParse(strManagerId, out managerId))
        //                    {
        //                        var manager = await _employeeRepository.GetByIdAsync(managerId);
        //                        if (manager == null)
        //                        {
        //                            managerId = 0;
        //                        }
        //                    }
        //                }

        //                if (!Enum.TryParse(worksheet.Cells[row, 26].Value?.ToString(), out EmployeeType employeeType))
        //                {
        //                    throw new Exception($"Invalid employeeType Value For Row number {row - 1}");
        //                }

        //                var employeeDto = new CreateEmployeeDTO
        //                {
        //                    Id = employeeId,
        //                    TeamId = teamEntity?.Id,
        //                    StateId = stateEntity?.Id,
        //                    FirstName = worksheet.Cells[row, 4].Value?.ToString() ?? "",
        //                    MiddleName = worksheet.Cells[row, 5].Value?.ToString() ?? "",
        //                    LastName = worksheet.Cells[row, 6].Value?.ToString() ?? "",
        //                    DateOfBirth = dobDate,
        //                    Gender = gender,
        //                    AadharNumber = worksheet.Cells[row, 10].Value?.ToString() ?? "",
        //                    PANNumber = worksheet.Cells[row, 11].Value?.ToString() ?? "",
        //                    ResidentialAddress = worksheet.Cells[row, 12].Value?.ToString() ?? "",
        //                    PermanentAddress = worksheet.Cells[row, 13].Value?.ToString() ?? "",
        //                    PersonalPhoneNumber = worksheet.Cells[row, 14].Value?.ToString() ?? "",
        //                    PersonalEmail = worksheet.Cells[row, 15].Value?.ToString() ?? "",
        //                    EmergencyContactNumber = worksheet.Cells[row, 16].Value?.ToString() ?? "",
        //                    OfficialEmail = worksheet.Cells[row, 17].Value?.ToString() ?? "",
        //                    ManagerId = managerId != 0 ? managerId : null,
        //                    RoleId = roleEntity?.Id,
        //                    DepartmentId = departmentEntity?.Id,
        //                    JoiningDate = DateTime.Parse(worksheet.Cells[row, 24].Value?.ToString() ?? ""),

        //                    EmployeeType = employeeType,
        //                };

        //                await CreateEmployeeAsync(employeeDto, true);
        //            }
        //        }
        //    }
        //    return true;
        //}

        public async Task<byte[]> ExportToExcelAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;  // ✅ Set EPPlus license context

            var employees = await _employeeRepository.GetAllAsync();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employees");

                // Headers
                worksheet.Cells[1, 1].Value = "Employee Id";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Middle Name";
                worksheet.Cells[1, 4].Value = "Last Name";

                int row = 2;
                foreach (var employee in employees)
                {
                    worksheet.Cells[row, 1].Value = employee.Id;
                    worksheet.Cells[row, 2].Value = employee.FirstName;
                    worksheet.Cells[row, 3].Value = employee.MiddleName;
                    worksheet.Cells[row, 4].Value = employee.LastName;

                    row++;
                }

                return package.GetAsByteArray();
            }
        }
    }
}
