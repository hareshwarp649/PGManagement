using bca.api.DTOs;
using bca.api.Models;
using Microsoft.AspNetCore.Identity;
using PropertyManage.Domain.DTOs;

namespace bca.api.Services
{
    public interface IUserService
    {
        
        //Task<IdentityResult> RegisterSPOC(RegisterSPOCModel model);
        //Task<IdentityResult> RegisterAdmin(RegisterAdminModel model);
        //Task<IdentityResult> RegisterSuperAdmin(RegisterAdminModel model);

        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName);
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> CreateAsync(UserCreateDTO dto);
        Task SoftDeleteAsync(Guid id);
        Task<UserDTO> RegisterUserAsync(RegisterUserModel model);
    }
}
