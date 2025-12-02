using ECommerceApp.EComm.Data.Context;
using ECommerceApp.EComm.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly EComDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(EComDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            var keyValues = id is object[] arr ? arr : new object[] { id };
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<T?> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            // Resolve primary key property via EF metadata
            var entityType = _context.Model.FindEntityType(typeof(T));
            var pk = entityType?.FindPrimaryKey();
            if (pk == null || pk.Properties.Count != 1)
            {
                throw new InvalidOperationException("GetByIdAsync with includes only supports single-column primary keys.");
            }

            var pkProperty = pk.Properties[0];
            var pkName = pkProperty.Name;

            // Build predicate dynamically: x => x.[pkName] == id
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, pkName);
            var constant = Expression.Constant(Convert.ChangeType(id, pkProperty.ClrType));
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private static Expression<Func<T, bool>> GetIdPredicate(int id)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "Id");
            var constant = Expression.Constant(id);
            var equality = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }
    }
}

