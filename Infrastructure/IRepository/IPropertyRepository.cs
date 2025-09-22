using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IPropertyRepository : IGenericRepository<Propertiy>
    {
        Task<bool> ExistsByNameAsync(string name, Guid clientId, Guid? ignoreId = null);
    }
}
