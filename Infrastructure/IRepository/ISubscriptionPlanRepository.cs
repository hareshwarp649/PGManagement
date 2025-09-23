using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface ISubscriptionPlanRepository : IGenericRepository<SubscriptionPlan>
    {
        Task<bool> ExistsByNameAsync(string planName);
    }

}
