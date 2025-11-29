using App.Application.Contracts.Persistence;

namespace App.Persistence {
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork {
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}