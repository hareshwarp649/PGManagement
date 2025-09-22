using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using System;

namespace PropertyManage.Infrastructure.Repository
{
    public class PropertyRepository : GenericRepository<Propertiy>, IPropertyRepository
    {
        private readonly ApplicationDbContext _context;
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid clientId, Guid? ignoreId = null)
        {
            return await _context.Properties
                .AnyAsync(p => p.PropertyName.ToLower() == name.ToLower()
                               && p.ClientId == clientId
                               && (!ignoreId.HasValue || p.Id != ignoreId.Value));
        }
    }
}
