using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Categories {
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, int>(context), ICategoryRepository {
        private new readonly AppDbContext _context = context;

        public Task<Category?> GetCategoryWithProductsAsync(int categoryId) =>
            _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == categoryId);

        public Task<List<Category>> GetCategoryWithProductsAsync() =>
            _context.Categories.Include(c => c.Products).ToListAsync();
    }
}