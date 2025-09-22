using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class ExpenseCategoryRepository : GenericRepository<ExpenseCategory>, IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string categoryName, Guid? ignoreId = null)
        {
            return await _context.ExpenseCategories
                .AnyAsync(ec => ec.CategoryName.ToLower() == categoryName.ToLower() && (!ignoreId.HasValue || ec.Id != ignoreId.Value));
        }
    }
}
