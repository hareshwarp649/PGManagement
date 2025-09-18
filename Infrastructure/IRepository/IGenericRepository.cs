using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace PropertyManage.Infrastructure.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<T?> GetByNameAsync(string name, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByColumnNameAndValueAsync(string columnName, string value, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task<bool> AddManyAsync(IEnumerable<T> entities);
        Task<T?> UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task<bool> DeleteAsync(Guid id);

        //void Update(T entity);
        Task<int> SaveChangesAsync();
        Task RemoveManyToManyAsync<TJoin>(Expression<Func<TJoin, bool>> predicate) where TJoin : class;
    }
}
