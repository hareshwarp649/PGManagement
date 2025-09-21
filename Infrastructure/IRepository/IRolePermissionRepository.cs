using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<IEnumerable<RolePermission>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task RemovePermissionsAsync(Guid roleId, List<Guid> permissionIds);
        Task AddRangeAsync(IEnumerable<RolePermission> rolePermissions);
    }
}
