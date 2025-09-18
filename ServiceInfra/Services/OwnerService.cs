using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _ownerRepository.GetAllAsync();
        }

        public async Task<Owner?> GetByIdAsync(Guid id)
        {
            return await _ownerRepository.GetByIdAsync(id);
        }

        public async Task<Owner?> CreateAsync(Owner owner)
        {
            return await _ownerRepository.AddAsync(owner);
        }

        public async Task<Owner?> UpdateAsync(Owner owner)
        {
            return await _ownerRepository.UpdateAsync(owner);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _ownerRepository.DeleteAsync(id);
        }

        public async Task<Owner?> GetOwnerWithPropertiesAsync(Guid ownerId)
        {
            return await _ownerRepository.GetOwnerWithPropertiesAsync(ownerId);
        }
    }
}
