using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country?> GetCountryWithStatesAsync(Guid countryId)
        {
            return await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == countryId);
        }

        public async Task<Country?> GetByCodeAsync(string code)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Code == code);
        }
    }
}
