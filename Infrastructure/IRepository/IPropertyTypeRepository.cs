using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
    {
        Task<bool> ExistsByNameAsync(string typeName);
    }
}
