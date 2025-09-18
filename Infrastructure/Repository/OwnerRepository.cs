using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        private readonly ApplicationDbContext _context;

        public OwnerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Owner?> GetOwnerWithPropertiesAsync(Guid ownerId)
        {
            return await _context.Owners
                .Include(o => o.Properties)
                .FirstOrDefaultAsync(o => o.Id == ownerId);
        }
    }
}
