using System.Linq.Expressions;
using App.Domain.Entities.Common;

namespace App.Application.Contracts.Persistence {
    public interface IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct {
        Task<bool> AnyAsync(TId id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllPagedAsync(int page, int pageSize);
        ValueTask<T?> GetByIdAsync(int id);
        ValueTask AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}