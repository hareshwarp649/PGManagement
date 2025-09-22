using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IPaymentModeRepository : IGenericRepository<PaymentMode>
    {
        Task<bool> ExistsByNameAsync(string modeName, Guid? ignoreId = null);
    }
}
