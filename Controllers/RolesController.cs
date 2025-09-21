
using bca.api.Services;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;

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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return role != null ? Ok(role) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDTO dto)
        {
            var role = await _roleService.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, RoleCreateDTO dto)
        {
            var updated = await _roleService.UpdateRoleAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet("{roleId}/permissions")]
        public async Task<IActionResult> GetPermissions(Guid roleId)
        {
            var permissions = await _rolePermissionService.GetPermissionsByRoleIdAsync(roleId);
            return Ok(permissions);
        }

        [HttpPost("{roleId}/permissions_assign")]
        public async Task<IActionResult> AddPermissions(Guid roleId, [FromBody] List<Guid> permissionIds)
        {
            var result = await _rolePermissionService.AddPermissionsAsync(roleId, permissionIds);
            if (!result)
                return BadRequest("No permissions added.");

            return Ok("Permissions added successfully.");
        }

        [HttpDelete("{roleId}/permissions_remove")]
        public async Task<IActionResult> RemovePermissions(Guid roleId, [FromBody] List<Guid> permissionIds)
        {
            await _rolePermissionService.RemovePermissionsAsync(roleId, permissionIds);
            return Ok("Permissions removed successfully.");
        }
    }
}
