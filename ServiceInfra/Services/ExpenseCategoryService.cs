using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;

        public ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository)
        {
           _expenseCategoryRepository = expenseCategoryRepository;
        }

        public async Task<IEnumerable<ExpenseCategory>> GetAllAsync()
        {
            return await _expenseCategoryRepository.GetAllAsync();
        }

        public async Task<ExpenseCategory> GetByIdAsync(Guid id)
        {
            return await _expenseCategoryRepository.GetByIdAsync(id);
        }

        public async Task<ExpenseCategory> CreateAsync(ExpenseCategory category)
        {
            if (await _expenseCategoryRepository.ExistsByNameAsync(category.CategoryName))
                throw new InvalidOperationException("Expense Category with the same name already exists.");

            await _expenseCategoryRepository.AddAsync(category);
            await _expenseCategoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task<ExpenseCategory> UpdateAsync(ExpenseCategory category)
        {
            if (await _expenseCategoryRepository.ExistsByNameAsync(category.CategoryName, category.Id))
                throw new InvalidOperationException("Expense Category with the same name already exists.");

            await _expenseCategoryRepository.UpdateAsync(category);
            await _expenseCategoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _expenseCategoryRepository.DeleteAsync(id);
        }
    }
}
