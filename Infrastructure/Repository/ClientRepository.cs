using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _context;
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null)
        {
            return await Query()
                .AnyAsync(c => c.ContactEmail.ToLower() == email.ToLower()
                               && (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task<bool> ExistsByNameAsync(string clientName)
        {
            return await _context.Clients.AnyAsync(c => c.ClientName == clientName);
        }
    }
}
