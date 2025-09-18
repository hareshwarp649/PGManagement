
using bca.api.DTOs;
using bca.api.Services;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;

namespace bca.api.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            return permission != null ? Ok(permission) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Permission permission)
        {
            var newPermission = await _permissionService.AddPermissionAsync(permission);
            return Ok(newPermission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Permission permission)
        {
            var updatedPermission = await _permissionService.UpdatePermissionAsync(id, permission);
            return updatedPermission != null ? Ok(updatedPermission) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> SearchAndSort(
            [FromQuery] string? name,
            [FromQuery] string? category,
            [FromQuery] string? description,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var models = await _permissionService.SearchAndSortPermissionsAsync(
                name, category, description, sortBy, sortOrder, pageNumber, pageSize
            );

            return Ok(models);
        }
    }
}
