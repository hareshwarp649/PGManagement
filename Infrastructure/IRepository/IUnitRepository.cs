using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IUnitRepository : IGenericRepository<Unit>
    {
        Task<bool> ExistsByUnitNumberAsync(Guid propertyId, string unitNumber, Guid? excludeId = null);
    }
}
