using App.Domain.Entities;

namespace App.Application.Contracts.Persistence {
    public interface ICategoryRepository : IGenericRepository<Category, int> {
        public Task<Category?> GetCategoryWithProductsAsync(int categoryId);
        Task<List<Category>> GetCategoryWithProductsAsync();
    }
}