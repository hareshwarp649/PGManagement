using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IClientSubscriptionService
    {
        Task<ClientSubscriptionDTO> CreateAsync(CreateClientSubscriptionDTO dto);
        Task<ClientSubscriptionDTO> UpdateAsync(Guid id, UpdateClientSubscriptionDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<ClientSubscriptionDTO>> GetAllClientSubscriptionsAsync();
        Task<ClientSubscriptionDTO> GetClientSubscriptionByIdAsync(Guid id);
    }
}
