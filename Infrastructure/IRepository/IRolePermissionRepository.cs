using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<IEnumerable<RolePermission>> GetPermissionsByRoleIdAsync(int roleId);
        Task RemovePermissionsAsync(int roleId, List<int> permissionIds);
        Task AddRangeAsync(IEnumerable<RolePermission> rolePermissions);
    }
}
