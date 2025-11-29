using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Response;
using App.Application.Features.Categories.Update;

namespace App.Application.Features.Categories {
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