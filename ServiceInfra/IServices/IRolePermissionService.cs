
using bca.api.DTOs;

namespace bca.api.Services
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task<bool> AddPermissionsAsync(Guid roleId, List<Guid> permissionIds);
        Task<bool> RemovePermissionsAsync(Guid roleId, List<Guid> permissionIds);
    }
}
