using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null);
    }
}
