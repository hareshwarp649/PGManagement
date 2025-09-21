using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<IEnumerable<Permission>> GetAllWithRolesAsync();
        
    }
}
