using App.Repositories.Categories;

namespace App.Services.Categories {
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public Task<Category?> GetCategoryWithProductsAsync(int categoryId) =>
            _categoryRepository.GetCategoryWithProductsAsync(categoryId);
    }
}