using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IRoleRepository : IGenericRepository<ApplicationRole>
    {
        Task<IEnumerable<ApplicationRole>> GetAllWithPermissionsAsync();
        Task<ApplicationRole?> GetByIdWithPermissionsAsync(Guid roleId);
    }
}
