
using bca.api.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.Repository;

namespace bca.api.Infrastructure.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Employee>, int)> SearchEmployeesAsync(string? name, int? roleId, int? departmentId, int? teamId, int? stateId,
            string? sortBy, string? sortOrder, int pageNumber, int pageSize)
        {
            var query = _context.Employees
                      .Include(e => e.Role)
                      .AsQueryable();
                query = query.Where(e => !e.IsDeleted);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(l => l.FirstName.Contains(name) || l.LastName.Contains(name));

            if (roleId.HasValue)
                query = query.Where(e => e.RoleId == roleId);

            //if (departmentId.HasValue)
            //    query = query.Where(e => e.DepartmentId == departmentId);

            //if (teamId.HasValue)
            //    query = query.Where(e => e.TeamId == teamId);

            //if (stateId.HasValue)
            //    query = query.Where(e => e.StateId == stateId);

            // ✅ Sorting
            bool isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortBy?.ToLower() switch
            {
                "firstname" => isDescending ? query.OrderByDescending(l => l.FirstName) : query.OrderBy(l => l.FirstName),
                "lastname" => isDescending ? query.OrderByDescending(l => l.LastName) : query.OrderBy(l => l.LastName),
                _ => query.OrderBy(l => l.Id) // Default sorting by ID
            };

            // ✅ Pagination
            int totalRecords = await query.CountAsync();
            var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (users, totalRecords);
        }
    }
}
