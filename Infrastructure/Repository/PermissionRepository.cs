using Microsoft.EntityFrameworkCore;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.Data;

namespace bca.api.Infrastructure.Repository
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllWithRolesAsync()
        {
            return await _context.Permissions
                .Include(p => p.RolePermissions)
                .ThenInclude(rp => rp.Role)
                .ToListAsync();
        }
        
    }
}
