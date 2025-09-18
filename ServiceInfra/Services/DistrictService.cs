using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IStateRepository _stateRepository;

        public DistrictService(IDistrictRepository districtRepository, IStateRepository stateRepository)
        {
            _districtRepository = districtRepository;
            _stateRepository = stateRepository;
        }

        public async Task<IEnumerable<District>> GetAllDistrictsAsync()
        {
            return await _districtRepository.GetAllAsync(null, q => q.Include(u => u.State));
        }

        public async Task<IEnumerable<District>> GetDistrictsByStateAsync(Guid stateId)
        {
            return await _districtRepository.GetByStateIdAsync(stateId);
        }

        public async Task<District?> GetDistrictByIdAsync(Guid id)
        {
            return await _districtRepository.GetByIdAsync(id, x => x.State);
        }

        public async Task<District> CreateDistrictAsync(District district)
        {
            return await _districtRepository.AddAsync(district);
        }

        public async Task<District?> UpdateDistrictAsync(District district)
        {
            return await _districtRepository.UpdateAsync(district);
        }

        public async Task<bool> DeleteDistrictAsync(Guid id)
        {
            return await _districtRepository.DeleteAsync(id);
        }     

        public async Task<District?> GetDistrictByStateAndDistrictNameAsync(Guid stateId, string districtName)
        {
            return await _districtRepository.GetByStateIdAndDistrictNameAsync(stateId, districtName);
        }
    }
}
