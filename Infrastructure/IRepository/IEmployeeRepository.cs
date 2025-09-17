using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<(IEnumerable<Employee>, int)> SearchEmployeesAsync(string? name, int? roleId, int? departmentId, int? teamId, int? stateId,
            string? sortBy, string? sortOrder, int pageNumber, int pageSize);
    }
}
