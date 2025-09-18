using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class CountryService:ICountryService
    {
        private readonly ICountryRepository _countryRepository;
    
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<Country?> CreateAsync(Country country)
        {
            return await _countryRepository.AddAsync(country);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _countryRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _countryRepository.GetAllAsync();
        }
        public async Task<Country?> GetByCodeAsync(string code)
        {
            return await _countryRepository.GetByCodeAsync(code);
        }
        public async Task<Country?> GetByIdAsync(Guid id)
        {
            return await _countryRepository.GetByIdAsync(id);
        }
        public async Task<Country?> GetCountryWithStatesAsync(Guid countryId)
        {   
            return await _countryRepository.GetCountryWithStatesAsync(countryId);
        }
        public async Task<Country?> UpdateAsync(Country country)
        {
            return await _countryRepository.UpdateAsync(country);
        }
    }
}

