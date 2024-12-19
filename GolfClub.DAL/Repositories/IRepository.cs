using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IAsyncEnumerable<T> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T?> TryGetByIdAsync(object id);
        Task<T?> TryGetAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        TContext GetContext<TContext>() where TContext : DbContext;
    }
}
