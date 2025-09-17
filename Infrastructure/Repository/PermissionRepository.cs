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

        public async Task<IEnumerable<Permission>> SearchAndSortPermissionsAsync(string? name, string? category, string? description, string? sortBy, string? sortOrder, int pageNumber, int pageSize)
        {
            var query = _context.Permissions
                .AsQueryable();

            // Apply Filters
            if (!string.IsNullOrEmpty(name))
                query = query.Where(b => b.Name.Contains(name));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(b => b.Category.Contains(category));

            if (!string.IsNullOrEmpty(description))
                query = query.Where(b => b.Description.Contains(description));

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool descending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

                query = sortBy.ToLower() switch
                {
                    "name" => descending ? query.OrderByDescending(b => b.Name) : query.OrderBy(b => b.Name),
                    "category" => descending ? query.OrderByDescending(b => b.Category) : query.OrderBy(b => b.Category),
                    "description" => descending ? query.OrderByDescending(b => b.Description) : query.OrderBy(b => b.Description),
                    _ => query.OrderBy(b => b.Id) // Default sorting by ID
                };
            }

            // Apply Pagination
            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(string userName, string permissionName)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(p => p.Permission)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return user?.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Any(p => p.Permission.Name == permissionName) ?? false;
        }
    }
}
