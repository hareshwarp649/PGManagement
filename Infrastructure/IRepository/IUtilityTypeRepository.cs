using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IUtilityTypeRepository : IGenericRepository<UtilityType>
    {
        Task<bool> ExistsByNameAsync(string utilityName, Guid? ignoreId = null);
    }
}
