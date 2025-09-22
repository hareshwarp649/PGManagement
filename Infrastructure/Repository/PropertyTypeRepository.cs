using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string typeName)
        {
            return await _context.PropertyTypes
                .AnyAsync(x => x.TypeName.ToLower() == typeName.ToLower());
        }
    }
}
