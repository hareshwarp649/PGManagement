using bca.api.DTOs;
using bca.api.Services;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create(UserCreateDTO dto)
        {
            try
            {
                var user = await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                await _userService.SoftDeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found" });
            }
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRoles(Guid userId)
        {
            var roles = await _userRoleService.GetRolesByUserIdAsync(userId);
            return Ok(roles); 
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole(UserRole userRole)
        {
            await _userRoleService.AssignRoleAsync(userRole);
            return Ok(new { message = "Role assigned successfully" });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole(UserRole userRole)
        {
            await _userRoleService.RemoveRoleAsync(userRole);
            return Ok(new { message = "Role removed successfully" });
        }
    }
}
