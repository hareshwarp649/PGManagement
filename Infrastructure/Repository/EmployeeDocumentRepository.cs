
using bca.api.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.Repository;

namespace bca.api.Infrastructure.Repository
{
    public class EmployeeDocumentRepository : GenericRepository<EmployeeDocument>, IEmployeeDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDocument>> GetEmployeeDocumentsAsync(int userId)
        {
            return await _context.EmployeeDocuments
                .Where(d => d.EmployeeId == userId)
                .ToListAsync();
        }
    }
}
