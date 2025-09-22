using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using System;

namespace PropertyManage.Infrastructure.Repository
{
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByUnitNumberAsync(Guid propertyId, string unitNumber, Guid? excludeId = null)
        {
            return await _context.Units
                .AnyAsync(u => u.PropertyId == propertyId
                            && u.UnitNumber.ToLower() == unitNumber.ToLower()
                            && (!excludeId.HasValue || u.Id != excludeId));
        }
    }
}
