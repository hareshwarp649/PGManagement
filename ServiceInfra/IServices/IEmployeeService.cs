using bca.api.DTOs;

namespace bca.api.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        //<IEnumerable<object>> GetAllEmployeesByStateIdAsync(int stateId);
        Task<IEnumerable<EmployeeDTO>> GetAllReportingEmployeesAsync(int managerId);

        Task<EmployeeDTO?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO employeeDto, bool updateIfExists = false);
        Task<EmployeeDTO?> UpdateEmployeeAsync(int id, UpdateEmployeeDTO employeeDto);
        Task<bool> ActivateDeactivateEmployeeAsync(int id, string action);
        Task<bool> DeleteEmployeeAsync(int id, bool softDelete = true);
        Task<(IEnumerable<EmployeeDTO>, int)> SearchEmployeesAsync(string? name, int? roleId, int? departmentId, int? teamId, int? stateId,
            string? sortBy, string? sortOrder, int pageNumber, int pageSize);
        Task<byte[]> ExportToExcelAsync();
    }
}
