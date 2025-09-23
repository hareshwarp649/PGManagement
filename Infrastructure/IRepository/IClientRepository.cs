using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null);
    }

}
