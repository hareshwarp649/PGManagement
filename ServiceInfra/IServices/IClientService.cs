using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDTO>> GetAllAsync();
        Task<ClientDTO?> GetByIdAsync(Guid id, Guid? requestingClientId = null, bool isSuperAdmin = false);
        Task<ClientDTO> CreateAsync(ClientCreateDTO dto);
        Task<ClientDTO?> UpdateAsync(Guid id, ClientUpdateDTO dto, bool isSuperAdmin = false, Guid? requestingClientId = null);
        Task<bool> DeleteAsync(Guid id, bool isSuperAdmin = false);
    }
}
