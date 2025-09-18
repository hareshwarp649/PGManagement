
using bca.api.Services;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;

namespace bca.api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IRolePermissionService _rolePermissionService;

        public RolesController(IRoleService roleService, IRolePermissionService rolePermissionService)
        {
            _roleService = roleService;
            _rolePermissionService = rolePermissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return role != null ? Ok(role) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Role role)
        {
            var newRole = await _roleService.AddRoleAsync(role);
            return Ok(newRole);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Role role)
        {
            var updatedRole = await _roleService.UpdateRoleAsync(id, role);
            return updatedRole != null ? Ok(updatedRole) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet("{roleId}/permissions")]
        public async Task<IActionResult> GetPermissions(int roleId)
        {
            var permissions = await _rolePermissionService.GetPermissionsByRoleIdAsync(roleId);
            return Ok(permissions);
        }

        [HttpPost("{roleId}/permissions")]
        public async Task<IActionResult> AddPermissions(int roleId, [FromBody] List<int> permissionIds)
        {
            var result = await _rolePermissionService.AddPermissionsAsync(roleId, permissionIds);
            if (!result)
                return BadRequest("No permissions added.");

            return Ok("Permissions added successfully.");
        }

        [HttpDelete("{roleId}/permissions")]
        public async Task<IActionResult> RemovePermissions(int roleId, [FromBody] List<int> permissionIds)
        {
            await _rolePermissionService.RemovePermissionsAsync(roleId, permissionIds);
            return Ok("Permissions removed successfully.");
        }
    }
}
