using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PropertyManage.Data;
using PropertyManage.Infrastructure.IRepository;
using System.Linq;
using System.Linq.Expressions;

namespace PropertyManage.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private DbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public GenericRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            // ✅ Apply nested Includes dynamically
            if (include != null)
            {
                query = include(query);
            }

            // ✅ Apply filtering AFTER includes
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _dbSet.Where(e => ids.Contains(EF.Property<Guid>(e, "Id"))).ToListAsync();
        }
        public virtual IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<T?> GetByNameAsync(string name, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);
        }

        public async Task<T?> GetByColumnNameAndValueAsync(string columnName, string value, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<string>(e, columnName) == value);
        }

        public async Task<T> AddAsync(T entity)
        {
            var nameProp = typeof(T).GetProperty("Name");
            if (nameProp != null)
            {
                var nameValue = nameProp.GetValue(entity)?.ToString();
                if (!string.IsNullOrEmpty(nameValue))
                {
                    bool exists = await _dbSet.AnyAsync(e =>
                        EF.Property<string>(e, "Name") == nameValue);

                    if (exists)
                        throw new Exception($"Entity with name '{nameValue}' already exists.");
                }
            }

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> AddManyAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return false;

            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RemoveManyToManyAsync<TJoin>(Expression<Func<TJoin, bool>> predicate) where TJoin : class
        {
            var entities = _context.Set<TJoin>().Where(predicate);
            _context.Set<TJoin>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
