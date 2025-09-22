using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IPropertyTypeService
    {
        Task<IEnumerable<PropertyTypeDTO>> GetAllAsync();
        Task<PropertyTypeDTO?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string typeName);
        Task<PropertyTypeDTO> CreateAsync(CreatePropertyTypeDTO dto);
        Task<PropertyTypeDTO?> UpdateAsync(PropertyTypeDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
