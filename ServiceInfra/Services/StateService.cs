using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }
        public async Task<State> AddStateAsync(State state)
        {
            return await _stateRepository.AddAsync(state);
        }

        public async Task<bool> DeleteStateAsync(Guid id)
        {
            return await _stateRepository.DeleteAsync((Guid)(object)id);
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await _stateRepository.GetAllAsync(include: q => q.Include(s => s.Country));
        }

        public async Task<State?> GetStateByDistrictIdAsync(Guid districtId)
        {
            return await _stateRepository.GetStateByDistrictIdAsync(districtId);
        }

        public async Task<State?> GetStateByIdAsync(Guid id)
        {
            return await _stateRepository.GetByIdAsync((Guid)(object)id, s => s.Country);
        }

        public async Task<IEnumerable<State>> GetStatesByCountryIdAsync(Guid countryId)
        {
            return await _stateRepository.GetStatesByCountryIdAsync(countryId);
        }

        public async Task<State?> UpdateStateAsync(State state)
        {
            return await _stateRepository.UpdateAsync(state);
        }
    }
}
