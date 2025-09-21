
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            return permission != null ? Ok(permission) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PermissionDTO permission)
        {
            var newPermission = await _permissionService.AddPermissionAsync(permission);
            return Ok(newPermission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PermissionDTO permission)
        {
            var updatedPermission = await _permissionService.UpdatePermissionAsync(id, permission);
            return updatedPermission != null ? Ok(updatedPermission) : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            return result ? Ok() : NotFound();
        }

        
    }
}
