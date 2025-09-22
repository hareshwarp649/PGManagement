using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class UtilityTypeRepository : GenericRepository<UtilityType>, IUtilityTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public UtilityTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string utilityName, Guid? ignoreId = null)
        {
            return await _context.UtilityTypes
                .AnyAsync(u => u.UtilityName.ToLower() == utilityName.ToLower()
                               && (!ignoreId.HasValue || u.Id != ignoreId.Value));
        }
    }
}
