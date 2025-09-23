using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync(Guid? clientId = null);
        Task<PropertyDTO> GetPropertyByIdAsync(Guid id, Guid? clientId = null);
        Task<PropertyDTO> AddPropertyAsync(PropertyCreateDTO dto);
        Task<PropertyDTO> UpdatePropertyAsync(Guid id, PropertyUpdateDTO dto, Guid clientId, bool isSuperAdmin = false);
        Task<bool> DeletePropertyAsync(Guid id, Guid clientId, bool isSuperAdmin = false);
    }
}
