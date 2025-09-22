using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class PaymentModeService : IPaymentModeService
    {
        private readonly IPaymentModeRepository _repository;

        public PaymentModeService(IPaymentModeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PaymentMode>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PaymentMode> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PaymentMode> CreateAsync(PaymentMode mode)
        {
            if (await _repository.ExistsByNameAsync(mode.ModeName))
                throw new InvalidOperationException($"Payment Mode '{mode.ModeName}' already exists.");

            await _repository.AddAsync(mode);
            await _repository.SaveChangesAsync();
            return mode;
        }

        public async Task<PaymentMode> UpdateAsync(PaymentMode mode)
        {
            if (await _repository.ExistsByNameAsync(mode.ModeName, mode.Id))
                throw new InvalidOperationException($"Payment Mode '{mode.ModeName}' already exists.");

            await _repository.UpdateAsync(mode);
            await _repository.SaveChangesAsync();
            return mode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
