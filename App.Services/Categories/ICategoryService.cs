using App.Services.Categories.Create;
using App.Services.Categories.Response;
using App.Services.Categories.Update;

namespace App.Services.Categories {
    public interface ICategoryService {
        public Task<ServiceResult<CategoryWithProductsResponse>> GetCategoryWithProductsAsync(int id);
        public Task<ServiceResult<List<CategoryWithProductsResponse>>> GetCategoryWithProductsAsync();
        public Task<ServiceResult<List<CategoryResponse>>> GetAllListAsync();
        public Task<ServiceResult<CategoryResponse>> GetByIdAsync(int categoryId);
        public Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest category);
        public Task<ServiceResult> UpdateAsync(UpdateCategoryRequest request);
        public Task<ServiceResult> DeleteAsync(int categoryId);
    }
}