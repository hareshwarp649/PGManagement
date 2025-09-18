using bca.api.DTOs;
using bca.api.Enums;
using bca.api.Helpers;
using bca.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.Enums;

namespace bca.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
       // private readonly ILocationService _locationService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
           
        }

        [HttpGet("employee-type-list")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<EnumValueDTO>> GetDocumentTypes()
        {
            var employeeTypes = EnumHelper.GetEnumValuesWithDescriptions<EmployeeType>()
                .ToList();

            return Ok(employeeTypes);
        }

        // ✅ Get All Employees
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
        {
            return Ok(await _employeeService.GetAllEmployeesAsync());
        }

        //[HttpGet("state/{stateId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<object>>> GetAllByStateId(int stateId)
        //{
        //    return Ok(await _employeeService.GetAllEmployeesByStateIdAsync(stateId));
        //}

        // ✅ Get All Reporting Employees
        [HttpGet("{managerId}/team")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllReportingEmployees(int managerId)
        {
            return Ok(await _employeeService.GetAllReportingEmployeesAsync(managerId));
        }

        // ✅ Get Employee by ID

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeDTO>> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        // ✅ Create New Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> Create([FromBody] CreateEmployeeDTO employeeDto)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);

            return CreatedAtAction(nameof(GetById), new { id = createdEmployee!.Id }, createdEmployee);
        }

       
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDTO employeeDto)
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, employeeDto);
            return updatedEmployee == null ? NotFound() : Ok(updatedEmployee);
        }

        [HttpPut("ActivateDeactivate")]
        public async Task<IActionResult> ActivateDeactivate([FromQuery] int id, [FromQuery] string action)
        {
            action = action.ToLower();
            var result = false;
            if (action == "activate" || action == "deactivate" )
            {
                result = await _employeeService.ActivateDeactivateEmployeeAsync(id, action);
            }
            return Ok(result);
        }

        // ✅ Delete Employee
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _employeeService.DeleteEmployeeAsync(id) ? NoContent() : NotFound();
        }

        // ✅ Search Endpoint
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Search(
            [FromQuery] string? name,
            [FromQuery] int? roleId,
            [FromQuery] int? departmentId,
            [FromQuery] int? teamId,
            [FromQuery] int? stateId,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (employees, totalRecords) = await _employeeService.SearchEmployeesAsync(name, roleId, departmentId, teamId, stateId,
                sortBy, sortOrder, pageNumber, pageSize);
            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Employees = employees
            });
        }

        //[HttpPost("upload-excel")]
        //[Consumes("multipart/form-data")]
        //[AllowAnonymous]
        //public async Task<IActionResult> UploadExcel(IFormFile file)
        //{
        //    try
        //    {
        //        var result = await _employeeService.BulkInsertFromExcelAsync(file);
        //        return result ? Ok("Data inserted successfully") : BadRequest("Invalid file.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (can use a logging library like Serilog or built-in logging)
        //        Console.WriteLine($"Error during Excel upload: {ex.Message}");
        //        return StatusCode(500, $"An error occurred while processing the file. {ex.Message}");
        //    }
        //}

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                var fileContent = await _employeeService.ExportToExcelAsync();
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Excel export: {ex.Message}");
                return StatusCode(500, "An error occurred while generating the Excel file.");
            }
        }
    }
}
