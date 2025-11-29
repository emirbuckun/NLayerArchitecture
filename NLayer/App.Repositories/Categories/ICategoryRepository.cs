namespace App.Repositories.Categories {
    public interface ICategoryRepository : IGenericRepository<Category, int> {
        public Task<Category?> GetCategoryWithProductsAsync(int categoryId);
        IQueryable<Category> GetCategoryWithProducts();
    }
}