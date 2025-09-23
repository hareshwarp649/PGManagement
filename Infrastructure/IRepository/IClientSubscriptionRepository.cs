using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IClientSubscriptionRepository : IGenericRepository<ClientSubscription>
    {
        Task<bool> ExistsByClientAndPlanAsync(Guid clientId, Guid planId);
    }
}
