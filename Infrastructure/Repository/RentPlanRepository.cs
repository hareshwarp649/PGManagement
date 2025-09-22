using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class RentPlanRepository : GenericRepository<RentPlan>, IRentPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public RentPlanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string planName, Guid? ignoreId = null)
        {
            return await _context.RentPlans
                .AnyAsync(rp => rp.PlanName.ToLower() == planName.ToLower()
                                && (!ignoreId.HasValue || rp.Id != ignoreId.Value));
        }
    }
}
