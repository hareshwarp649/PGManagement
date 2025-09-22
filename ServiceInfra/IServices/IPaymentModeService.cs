using PropertyManage.Data.MasterEntities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IPaymentModeService
    {
        Task<IEnumerable<PaymentMode>> GetAllAsync();
        Task<PaymentMode> GetByIdAsync(Guid id);
        Task<PaymentMode> CreateAsync(PaymentMode mode);
        Task<PaymentMode> UpdateAsync(PaymentMode mode);
        Task<bool> DeleteAsync(Guid id);
    }
}
