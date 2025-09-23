using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class ClientSubscriptionRepository : GenericRepository<ClientSubscription>, IClientSubscriptionRepository
    {
        public ClientSubscriptionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsByClientAndPlanAsync(Guid clientId, Guid planId)
        {
            return await Query().AnyAsync(cs => cs.ClientId == clientId && cs.SubscriptionPlanId == planId);
        }
    }
}
