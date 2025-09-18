using bca.api.DTOs;
using bca.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bca.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public UsersController(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }

        [HttpGet("role/{roleName}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRole(string roleName)
        {
            var users = await _userService.GetUsersByRoleAsync(roleName);
            return Ok(users);
        }

        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _userRoleService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AddRoles(string userId, [FromBody] List<int> roleIds)
        {
            var result = await _userRoleService.AddRolesAsync(userId, roleIds);
            if (!result)
                return BadRequest("No roles added.");

            return Ok("Roles added successfully.");
        }

        [HttpDelete("{userId}/roles")]
        public async Task<IActionResult> RemoveRoles(string userId, [FromBody] List<int> roleIds)
        {
            await _userRoleService.RemoveRolesAsync(userId, roleIds);
            return Ok("Roles removed successfully.");
        }
    }
}
