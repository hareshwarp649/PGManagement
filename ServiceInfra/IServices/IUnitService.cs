using PropertyManage.Domain.DTOs;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IUnitService
    {
        Task<UnitDTO> CreateAsync(UnitCreateDTO dto, Guid currentUserId);
        Task<UnitDTO> UpdateAsync(Guid id, UnitUpdateDTO dto, Guid currentUserId);
        Task<bool> DeleteAsync(Guid id);
        Task<UnitDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<UnitDTO>> GetAllByPropertyAsync(Guid propertyId);
    }
}
