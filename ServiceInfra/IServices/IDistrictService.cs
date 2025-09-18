using PropertyManage.Data.Entities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IDistrictService
    {
        Task<IEnumerable<District>> GetAllDistrictsAsync();
        Task<IEnumerable<District>> GetDistrictsByStateAsync(Guid stateId);
        Task<District?> GetDistrictByIdAsync(Guid id);
        Task<District> CreateDistrictAsync(District district);
        Task<District?> UpdateDistrictAsync(District district);
        Task<bool> DeleteDistrictAsync(Guid id);
        Task<District?> GetDistrictByStateAndDistrictNameAsync(Guid stateId, string districtName);
    }
}
