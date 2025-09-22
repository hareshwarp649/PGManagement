using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class UtilityTypeService: IUtilityTypeService
    {
        private readonly IUtilityTypeRepository _utilityTypeRepository;

        public UtilityTypeService(IUtilityTypeRepository utilityTypeRepository)
        {
            _utilityTypeRepository = utilityTypeRepository;
        }

        public async Task<IEnumerable<Data.MasterEntities.UtilityType>> GetAllAsync()
        {
            return await _utilityTypeRepository.GetAllAsync();
        }

        public async Task<UtilityType?> GetByIdAsync(Guid id)
        {
            return await _utilityTypeRepository.GetByIdAsync(id);
        }

        public async Task<UtilityType> CreateAsync(UtilityType utilityType)
        {
            if (await _utilityTypeRepository.ExistsByNameAsync(utilityType.UtilityName))
            {
                throw new InvalidOperationException("A utility type with the same name already exists.");
            }
            return await _utilityTypeRepository.AddAsync(utilityType);
        }

        public async Task<UtilityType?> UpdateAsync(UtilityType utilityType)
        {
            if (await _utilityTypeRepository.ExistsByNameAsync(utilityType.UtilityName, utilityType.Id))
                throw new InvalidOperationException($"Utility Type '{utilityType.UtilityName}' already exists.");
           return  await _utilityTypeRepository.UpdateAsync(utilityType);

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _utilityTypeRepository.DeleteAsync(id);
        }
    }
}
