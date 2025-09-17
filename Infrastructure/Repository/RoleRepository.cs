using Microsoft.EntityFrameworkCore;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.Data;
using PropertyManage.Data.Entities;

namespace bca.api.Infrastructure.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllWithPermissionsAsync()
        {
            return await _context.Roles.Include(r => r.RolePermissions)
                               .ThenInclude(rp => rp.Permission)
                               .ToListAsync();
        }
    }
}
