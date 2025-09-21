using Microsoft.EntityFrameworkCore;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.Data;
using PropertyManage.Data.Entities;

namespace bca.api.Infrastructure.Repository
{
    public class RoleRepository : GenericRepository<ApplicationRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllWithPermissionsAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<ApplicationRole?> GetByIdWithPermissionsAsync(Guid roleId)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }
    }
}
