using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class RentPlanService : IRentPlanService
    {
       private readonly IRentPlanRepository _rentPlanRepository;
        public RentPlanService(IRentPlanRepository rentPlanRepository)
        {
            _rentPlanRepository = rentPlanRepository;
        }
        public async Task<IEnumerable<RentPlan>> GetAllAsync()
        {
            return await _rentPlanRepository.GetAllAsync();
        }
        public async Task<RentPlan?> GetByIdAsync(Guid id)
        {
            return await _rentPlanRepository.GetByIdAsync(id);
        }
        public async Task<RentPlan> CreateAsync(RentPlan rentPlan)
        {
            if (await _rentPlanRepository.ExistsByNameAsync(rentPlan.PlanName))
            {
                throw new InvalidOperationException("A rent plan with the same name already exists.");
            }
            return await _rentPlanRepository.AddAsync(rentPlan);
        }
        public async Task<RentPlan?> UpdateAsync(RentPlan rentPlan)
        {
            if (await _rentPlanRepository.ExistsByNameAsync(rentPlan.PlanName, rentPlan.Id))
                throw new InvalidOperationException($"Rent Plan '{rentPlan.PlanName}' already exists.");

           return  await _rentPlanRepository.UpdateAsync(rentPlan);
           
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _rentPlanRepository.DeleteAsync(id);
        }
    }
}
