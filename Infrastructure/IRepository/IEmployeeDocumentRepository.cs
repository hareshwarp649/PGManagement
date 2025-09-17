
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IEmployeeDocumentRepository : IGenericRepository<EmployeeDocument>
    {
        Task<IEnumerable<EmployeeDocument>> GetEmployeeDocumentsAsync(int userId);
    }
}
