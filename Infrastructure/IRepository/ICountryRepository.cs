using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country?> GetCountryWithStatesAsync(Guid countryId);
        Task<Country?> GetByCodeAsync(string code);
    }
}
