using System.Linq.Expressions;
using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence {
    public class GenericRepository<T, TId>(AppDbContext context) :
        IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct {
        protected readonly AppDbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<bool> AnyAsync(TId id) => _dbSet.AnyAsync(x => x.Id.Equals(id));

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

        public async Task<List<T>> GetAllAsync() => await _dbSet.AsQueryable().AsNoTracking().ToListAsync();

        public async Task<List<T>> GetAllPagedAsync(int page, int pageSize) =>
            await _dbSet.AsQueryable().AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

        public async ValueTask<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}