
using bca.api.DTOs;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;

namespace bca.api.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO?> GetRoleByIdAsync(Guid id);
        //Task<RoleDTO?> GetRoleByNameAsync(string name);
        Task<RoleDTO> CreateRoleAsync(RoleCreateDTO dto);
        Task<RoleDTO?> UpdateRoleAsync(Guid id, RoleCreateDTO dto);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}
