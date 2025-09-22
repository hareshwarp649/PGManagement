using PropertyManage.Data.MasterEntities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IRentPlanService
    {
        Task<IEnumerable<RentPlan>> GetAllAsync();
        Task<RentPlan?> GetByIdAsync(Guid id);
        Task<RentPlan> CreateAsync(RentPlan rentPlan);
        Task<RentPlan?> UpdateAsync(RentPlan rentPlan);
        Task<bool> DeleteAsync(Guid id);
    }
}
