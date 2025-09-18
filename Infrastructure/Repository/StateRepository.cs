using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.Repository;

namespace PropertyManage.Infrastructure.IRepository
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        ApplicationDbContext _context;

        public StateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ Get all states for a specific country
        public async Task<IEnumerable<State>> GetStatesByCountryIdAsync(Guid countryId)
        {
            return await GetAllAsync(
                filter: s => s.CountryId == countryId,
                include: query => query.Include(s => s.Country)
            );
        }

        // ✅ Get state by districtId (reverse lookup)
        public async Task<State?> GetStateByDistrictIdAsync(Guid districtId)
        {
            return await _context.States
                .Include(s => s.Districts)
                .FirstOrDefaultAsync(s => s.Districts.Any(d => d.Id == districtId));
        }
       
    }
}
