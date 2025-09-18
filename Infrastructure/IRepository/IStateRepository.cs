using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IStateRepository : IGenericRepository<State>
    {
        Task<IEnumerable<State>> GetStatesByCountryIdAsync(Guid countryId);
        Task<State?> GetStateByDistrictIdAsync(Guid districtId);

    }
}
