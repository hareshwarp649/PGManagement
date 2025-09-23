using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionPlanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string planName)
        {
            return await _context.Set<SubscriptionPlan>()
                                 .AnyAsync(x => x.PlanName.ToLower() == planName.ToLower());
        }
    }
   }
