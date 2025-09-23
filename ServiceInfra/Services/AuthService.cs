using bca.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using PropertyManage.ServiceInfra.IServices;
using PropertyManage.Data;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace PropertyManage.ServiceInfra.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> um ,ApplicationDbContext context, IConfiguration cfg, ILogger<AuthService> logger)
        {
            _userManager = um;
            _context = context;
            _config = cfg; 
            _logger = logger;
        }


        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto, string deviceInfo)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await GetUserPermissionsAsync(user.Id);

            var (token, expiresAt, jti) = CreateAccessToken(user, roles, permissions);
            var refresh = CreateRefreshToken();

            var rt = new ApplicationUserRefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Hash(refresh),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                DeviceInfo = deviceInfo
            };
            _context.RefreshTokens.Add(rt);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = token,
                RefreshToken = refresh,
                AccessTokenExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDTO?> RefreshAsync(string refreshToken, string deviceInfo)
        {
            var hashed = Hash(refreshToken);
            var existing = await _context.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == hashed);

            if (existing == null || existing.Revoked || existing.ExpiresAt <= DateTime.UtcNow)
                return null;

            // rotate: revoke existing and replace
            existing.Revoked = true;
            existing.ReplacedByToken = Guid.NewGuid().ToString(); // simple chain id
            _context.RefreshTokens.Update(existing);

            var user = existing.User;
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await GetUserPermissionsAsync(user.Id);

            var (newToken, expiresAt, jti) = CreateAccessToken(user, roles, permissions);
            var newRefreshPlain = CreateRefreshToken();

            var newRefresh = new ApplicationUserRefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Hash(newRefreshPlain),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                DeviceInfo = deviceInfo,
                ReplacedByToken = null
            };
            _context.RefreshTokens.Add(newRefresh);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = newToken,
                RefreshToken = newRefreshPlain,
                AccessTokenExpiresAt = expiresAt
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var hashed = Hash(refreshToken);
            var existing = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == hashed);
            if (existing != null)
            {
                existing.Revoked = true;
                _context.RefreshTokens.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        // Helper methods
        private (string token, DateTime expiresAt, string jti) CreateAccessToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            var jwtCfg = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtCfg["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jti = Guid.NewGuid().ToString();
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(double.Parse(jwtCfg["AccessMinutes"] ?? "15"));

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim("clientId", user.ClientId?.ToString() ?? Guid.Empty.ToString())
        };
            foreach (var r in roles) claims.Add(new Claim(ClaimTypes.Role, r));
            foreach (var p in permissions) claims.Add(new Claim("permission", p));

            var token = new JwtSecurityToken(
                issuer: jwtCfg["Issuer"],
                audience: jwtCfg["Audience"],
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires, jti);
        }

        private string CreateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            var data = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(data);
        }

        private async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
        {
            var perms = await (from ur in _context.UserRoles
                               join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                               join p in _context.Permissions on rp.PermissionId equals p.Id
                              // where ur.UserId == userId && ur.IsActive
                               select p.Name).Distinct().ToListAsync();
            return perms;
        }
    }
}
