using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IDistrictRepository : IGenericRepository<District>
    {
        Task<IEnumerable<District>> GetByStateIdAsync(Guid stateId);
        Task<District?> GetByStateIdAndDistrictNameAsync(Guid stateId, string districtName);
    }
}
