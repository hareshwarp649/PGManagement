using PropertyManage.Data.Entities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        Task<Owner?> GetOwnerWithPropertiesAsync(Guid ownerId);
    }
}
