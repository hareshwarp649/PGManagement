using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IExpenseCategoryRepository : IGenericRepository<ExpenseCategory>
    {
        Task<bool> ExistsByNameAsync(string categoryName, Guid? ignoreId = null);
    }
}
