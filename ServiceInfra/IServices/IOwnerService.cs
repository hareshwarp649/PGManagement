using PropertyManage.Data.Entities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(Guid id);
        Task<Owner?> CreateAsync(Owner owner);
        Task<Owner?> UpdateAsync(Owner owner);
        Task<bool> DeleteAsync(Guid id);
        Task<Owner?> GetOwnerWithPropertiesAsync(Guid ownerId);
    }
}
