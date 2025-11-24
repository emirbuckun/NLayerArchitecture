using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Categories {
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository {
        private new readonly AppDbContext _context = context;

        public Task<Category?> GetCategoryWithProductsAsync(int categoryId) =>
            _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == categoryId);

        public IQueryable<Category> GetCategoryWithProducts() =>
            _context.Categories.Include(c => c.Products).AsQueryable();
    }
}