using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface ISubscriptionPlanService
    {
        Task<SubscriptionPlanDTO> CreateAsync(SubscriptionPlanCreateDTO dto);
        Task<SubscriptionPlanDTO> UpdateAsync(Guid id, SubscriptionPlanUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<SubscriptionPlanDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<SubscriptionPlanDTO>> GetAllAsync();
    }
}
