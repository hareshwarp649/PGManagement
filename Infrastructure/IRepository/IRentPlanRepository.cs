using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IRentPlanRepository : IGenericRepository<RentPlan>
    {
        Task<bool> ExistsByNameAsync(string planName, Guid? ignoreId = null);
    }
}
