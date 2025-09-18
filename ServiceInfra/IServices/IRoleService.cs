
using bca.api.DTOs;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(Guid id);
        Task<Role?> GetRoleByNameAsync(string name);
        Task<Role> AddRoleAsync(Role role);
        Task<Role?> UpdateRoleAsync(Guid id, Role role);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}
