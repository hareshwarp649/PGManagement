using PropertyManage.Data.Entities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface ICountryService
    {

        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(Guid id);

        Task<Country?> GetByCodeAsync(string code);
        Task<Country?> GetCountryWithStatesAsync(Guid countryId);
        Task<Country?> CreateAsync(Country country);
        Task<Country?> UpdateAsync(Country country);
        Task<bool> DeleteAsync(Guid id);


    }
}
