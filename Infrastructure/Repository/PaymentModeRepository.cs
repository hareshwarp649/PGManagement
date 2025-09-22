using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class PaymentModeRepository : GenericRepository<PaymentMode>, IPaymentModeRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentModeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string modeName, Guid? ignoreId = null)
        {
            return await _context.PaymentModes
                .AnyAsync(pm => pm.ModeName.ToLower() == modeName.ToLower()
                                && (!ignoreId.HasValue || pm.Id != ignoreId.Value));
        }
    }
}
