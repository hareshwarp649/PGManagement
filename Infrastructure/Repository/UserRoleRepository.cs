using Microsoft.EntityFrameworkCore;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.Data;
using PropertyManage.Data.Entities;

namespace bca.api.Infrastructure.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserRole>> GetUsersByRoleNameAsync(string roleName)
        {
            return await _context.UserRoles
                .Where(ur => ur.Role.Name == roleName)
                .Include(ur => ur.User)
                .ToListAsync();
        }


        public async Task AssignRoleAsync(UserRole userRole)
        {
            var exists = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);
            if (!exists)
            {
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveRoleAsync(UserRole userRole)
        {
            var existing = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);
            if (existing != null)
            {
                _context.UserRoles.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ApplicationRole>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .ToListAsync();
        }
    }
}
