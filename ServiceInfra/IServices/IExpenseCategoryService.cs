using PropertyManage.Data.MasterEntities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IExpenseCategoryService
    {
        Task<IEnumerable<ExpenseCategory>> GetAllAsync();
        Task<ExpenseCategory> GetByIdAsync(Guid id);
        Task<ExpenseCategory> CreateAsync(ExpenseCategory category);
        Task<ExpenseCategory> UpdateAsync(ExpenseCategory category);
        Task<bool> DeleteAsync(Guid id);
    }
}
