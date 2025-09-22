using PropertyManage.Data.MasterEntities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IUtilityTypeService
    {
        Task<IEnumerable<UtilityType>> GetAllAsync();
        Task<UtilityType?> GetByIdAsync(Guid id);
        Task<UtilityType> CreateAsync(UtilityType utilityType);
        Task<UtilityType?> UpdateAsync(UtilityType utilityType);
        Task<bool> DeleteAsync(Guid id);
    }
}
