
using bca.api.Enums;
using Microsoft.AspNetCore.Identity;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Enums;
using System.Security.Claims;

namespace bca.api.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserContextService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public string UserId => _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
        //public string UserName => _contextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        public string? UserName => _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        public Guid? ClientId
        {
            get
            {
                var claim = _contextAccessor.HttpContext?.User?.FindFirst("clientId")?.Value;
                if (Guid.TryParse(claim, out var gid)) return gid;
                return null;
            }
        }

    }

}
