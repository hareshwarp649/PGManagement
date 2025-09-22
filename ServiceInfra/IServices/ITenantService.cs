using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface ITenantService
    {
        Task<TenantDTO> CreateAsync(TenantCreateDTO dto);
        Task<TenantDTO> UpdateAsync(Guid id, TenantUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<TenantDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<TenantDTO>> GetAllAsync();
    }
}
