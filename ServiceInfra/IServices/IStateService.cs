using PropertyManage.Data.Entities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetAllStatesAsync();
        Task<IEnumerable<State>> GetStatesByCountryIdAsync(Guid countryId);
        Task<State?> GetStateByDistrictIdAsync(Guid districtId);
        Task<State?> GetStateByIdAsync(Guid id);
        Task<State> AddStateAsync(State state);
        Task<State?> UpdateStateAsync(State state);
        Task<bool> DeleteStateAsync(Guid id);
    }
}
