using bca.api.DTOs;

namespace bca.api.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<RoleDTO>> GetUserRolesAsync(string userId);
        Task<bool> AddRolesAsync(string userId, List<int> roleIds);
        Task<bool> RemoveRolesAsync(string userId, List<int> roleIds);
        Task<bool> DeleteRolesAsync(string userId);
    }
}
