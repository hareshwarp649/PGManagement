using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        private readonly ApplicationDbContext _context;

        public DistrictRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<District>> GetByStateIdAsync(Guid stateId)
        {
            return await _context.Districts.Include(l => l.State).Where(d => d.StateId == stateId).ToListAsync();
        }

        public async Task<District?> GetByStateIdAndDistrictNameAsync(Guid stateId, string districtName)
        {
            return await _context.Districts.Include(l => l.State)
                .FirstOrDefaultAsync(d => d.StateId == stateId && d.Name == districtName);
        }
    }
}
