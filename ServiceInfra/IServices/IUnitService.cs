using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDTO>> GetAllAsync();
        Task<UnitDTO> CreateAsync(UnitCreateDTO dto);
        Task<UnitDTO> UpdateAsync(Guid id, UnitUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<UnitDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<UnitDTO>> GetAllByPropertyAsync(Guid propertyId);
    }
}
