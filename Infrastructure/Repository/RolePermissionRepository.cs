using Microsoft.EntityFrameworkCore;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.Data.Entities;

namespace bca.api.Infrastructure.Repository
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public RolePermissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task RemovePermissionsAsync(int roleId, List<int> permissionIds)
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
            .ToListAsync();

            _context.RolePermissions.RemoveRange(permissions);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<RolePermission> rolePermissions) // ✅ Add this method
        {
            await _context.RolePermissions.AddRangeAsync(rolePermissions); // Bulk Insert
            await _context.SaveChangesAsync();
        }
    }
}
