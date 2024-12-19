using System.Linq.Expressions;
using GolfClub.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.DAL.Repositories
{
    public class GenericRepository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async IAsyncEnumerable<T> GetAllAsync()
        {
            await foreach (var entity in _dbSet.AsAsyncEnumerable())
            {
                yield return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

        public async Task<T?> TryGetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task<T?> TryGetAsync(Expression<Func<T, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public void Update(T entity) => context.Entry(entity).State = EntityState.Modified;

        public void Delete(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            
            _dbSet.Remove(entity);
        }

        public Task SaveChangesAsync() => context.SaveChangesAsync();

        public TContext GetContext<TContext>() where TContext : DbContext => context as TContext;
    }
}

