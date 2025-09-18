using bca.api.DTOs;
using bca.api.Models;
using Microsoft.AspNetCore.Identity;

namespace bca.api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName);
        Task<IdentityResult> RegisterSPOC(RegisterSPOCModel model);
        Task<IdentityResult> RegisterAdmin(RegisterAdminModel model);
        Task<IdentityResult> RegisterSuperAdmin(RegisterAdminModel model);
    }
}
