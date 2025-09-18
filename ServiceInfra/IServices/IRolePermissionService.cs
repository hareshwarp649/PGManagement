
using bca.api.DTOs;

namespace bca.api.Services
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleIdAsync(int roleId);
        Task<bool> AddPermissionsAsync(int roleId, List<int> permissionIds);
        Task<bool> RemovePermissionsAsync(int roleId, List<int> permissionIds);
    }
}
