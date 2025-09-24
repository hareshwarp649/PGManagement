
using bca.api.DTOs;
using bca.api.Models;
using bca.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Enums;
using PropertyManage.ServiceInfra.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bca.api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserContextService _userContextService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuthService authService ,IUserRoleService userRoleService, IConfiguration configuration, IUserService userService, IUserContextService userContextService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _userRoleService = userRoleService;
            _configuration = configuration;
            _userService = userService;
            _userContextService = userContextService;
        }



        //[HttpPost("register-admin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminModel model)
        //{
        //    var result = await _userService.RegisterAdmin(model);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(result.Errors);
        //    }

        //    return Ok(new { message = "Admin registered successfully!" });
        //}

        //[HttpPost("register-superadmin")]
        //public async Task<IActionResult> RegisterSuperAdmin([FromBody] RegisterAdminModel model)
        //{
        //    var result = await _userService.RegisterSuperAdmin(model);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(result.Errors);
        //    }

        //    return Ok(new { message = "SuperAdmin registered successfully!" });
        //}

        //[HttpPost("admin/change-password")]
        //public async Task<IActionResult> AdminChangePassword([FromBody] UpdatePasswordDTO model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);
        //    if (user == null) return NotFound("User not found.");

        //    var removeResult = await _userManager.RemovePasswordAsync(user);
        //    if (!removeResult.Succeeded)
        //        return BadRequest("Failed to remove old password.");

        //    var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
        //    if (!addResult.Succeeded)
        //        return BadRequest(addResult.Errors);

        //    return Ok("Password changed successfully.");
        //}

        //[HttpDelete("admin/delete-user/{userId}")]
        //public async Task<IActionResult> AdminDeleteUser(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null) return NotFound("User not found.");

        //    await _userRoleService.DeleteRolesAsync(user.Id);
        //    var result = await _userManager.DeleteAsync(user);
        //    if (!result.Succeeded)
        //        return BadRequest(result.Errors);

        //    return Ok("User deleted successfully.");
        //}

        //[HttpPost("login")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login([FromBody] LoginDTO model)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        //    if (!result.Succeeded)
        //    {
        //        return Unauthorized();
        //    }

        //    var user = await _userManager.FindByNameAsync(model.Username);

        //    if (user == null || user.IsDeleted)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        //    if (!passwordValid)
        //        return Unauthorized("Invalid password.");

        //    var roles = await _userManager.GetRolesAsync(user);
        //    var token = GenerateJwtToken(user, roles);

        //    return Ok(new { token, user.Id, user.EntityId, isAdmin = user.UserType == UserType.Admin });
        //}


        //[Authorize]
        //[HttpGet("user")]
        //public async Task<IActionResult> GetCurrentUser()
        //{
        //    var user = await _userContextService.GetCurrentUserAsync(true, true);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }

        //    EmployeeDTO employee = null;
        //    if (user.EntityId.HasValue)
        //    {
        //        employee = await _employeeService.GetEmployeeByIdAsync(user.EntityId.Value);
        //        if (employee == null || employee.IsDeleted)
        //        {
        //            return NotFound("Employee not found.");
        //        }
        //    }

        //    return Ok(new
        //    {
        //        user.Id,
        //        user.UserName,
        //        user.Email,
        //        user.PhoneNumber,
        //        user.EntityId,
        //        //user.BankId,
        //        Name = employee != null ? $"{employee.FirstName} {employee.LastName}" : user.UserName,
        //        Roles = (await _userRoleService.GetUserRolesAsync(user.Id)).Select(r => new{ RoleId = r.Id, RoleName = r.Name}).ToArray()
        //    });
        //}

        //[Authorize]
        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        //{
        //    var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var user = await _userManager.FindByNameAsync(userName!);

        //    if (model == null)
        //        return BadRequest("Invalid request.");

        //    if (user == null || user.IsDeleted)
        //        return NotFound("User not found.");

        //    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(result.Errors);
        //    }

        //    return Ok("Password changed successfully.");
        //}

        //private async Task<string> GenerateJwtToken(ApplicationUser user, IList<string> roles)
        //{
        //    var userRoles = await _userRoleService.GetUserRolesAsync(user.Id);

        //    var jwtSettings = _configuration.GetSection("JwtSettings");
        //    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var claims = new List<Claim>()
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    foreach (var userRole in userRoles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, userRole.Name!));
        //    }

        //    var token = new JwtSecurityToken(
        //    issuer: jwtSettings["Issuer"],
        //    audience: jwtSettings["Audience"],
        //    claims: claims,
        //    expires: DateTime.Now.AddMinutes(30),
        //    signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        
        [HttpPost("register-superadmin")]
        [AllowAnonymous] 
        public async Task<IActionResult> RegisterSuperAdmin([FromBody] RegisterUserModel model)
        {
            try
            {
                model.RoleName = "SuperAdmin";
                model.UserType = UserType.SuperAdmin;

                var userDto = await _userService.RegisterUserAsync(model);

                return Ok(new
                {
                    Message = "SuperAdmin registered successfully",
                    User = userDto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
       
        [HttpPost("register-admin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserModel model)
        {
            if (model == null)
                return BadRequest("Model cannot be null");

            if (!model.ClientId.HasValue)
                return BadRequest("ClientId is required");

            try
            {
                model.RoleName = "Admin";
                model.UserType = UserType.Admin;

                var userDto = await _userService.RegisterUserAsync(model);

                return Ok(new
                {
                    Message = "Admin registered successfully",
                    User = userDto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var device = Request.Headers["User-Agent"].ToString();
                var res = await _authService.LoginAsync(dto, device);

                if (res.MustChangePassword)
                    return Ok(new
                    {
                        accessToken = res.AccessToken,
                        accessTokenExpiresAt = res.AccessTokenExpiresAt,
                        mustChangePassword = true,
                        message = res.Message
                    });

                return Ok(res);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Password change required"))
            {
                return BadRequest(new { message = ex.Message, mustChangePassword = true });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("change-password")]      
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var userId = _userContextService.UserGuidId;
            if (userId == null)
                return Unauthorized(new { message = "Invalid or missing user identity." });

            //var user = await _userManager.FindByIdAsync(_userContextService.UserId.ToString());
            //if (user == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user == null)
                return Unauthorized(new { message = "User not found." });

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Remove flag after first password change
            if (user.MustChangePassword)
            {
                user.MustChangePassword = false;
                await _userManager.UpdateAsync(user);
            }

            return Ok("Password changed successfully.");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var device = Request.Headers["User-Agent"].ToString();
            var res = await _authService.RefreshAsync(refreshToken, device);
            if (res == null) return Unauthorized();
            return Ok(res);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _authService.LogoutAsync(refreshToken);
            return NoContent();
        }
    }
}
