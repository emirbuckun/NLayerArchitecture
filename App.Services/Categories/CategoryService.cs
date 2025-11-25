using System.Net;
using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Response;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Categories {
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService {
        public async Task<ServiceResult<CategoryWithProductsResponse>> GetCategoryWithProductsAsync(int id) {
            var category = await categoryRepository.GetCategoryWithProductsAsync(id);
            if (category == null) {
                return ServiceResult<CategoryWithProductsResponse>.Fail("Category not found", HttpStatusCode.NotFound)!;
            }

            var categoryResponse = mapper.Map<CategoryWithProductsResponse>(category);

            return ServiceResult<CategoryWithProductsResponse>.Success(categoryResponse);
        }

        public async Task<ServiceResult<List<CategoryWithProductsResponse>>> GetCategoryWithProductsAsync() {
            var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();

            var categoryResponse = mapper.Map<List<CategoryWithProductsResponse>>(category);

            return ServiceResult<List<CategoryWithProductsResponse>>.Success(categoryResponse);
        }

        public async Task<ServiceResult<List<CategoryResponse>>> GetAllListAsync() {
            var categories = await categoryRepository.GetAll().ToListAsync();

            var categoryResponses = mapper.Map<List<CategoryResponse>>(categories);

            return ServiceResult<List<CategoryResponse>>.Success(categoryResponses);
        }

        public async Task<ServiceResult<CategoryResponse>> GetByIdAsync(int categoryId) {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return ServiceResult<CategoryResponse>.Fail("Category not found.", HttpStatusCode.NotFound);

            var categoryResponse = mapper.Map<CategoryResponse>(category);
            return ServiceResult<CategoryResponse>.Success(categoryResponse);
        }

        public async Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request) {
            var isSameCategoryExist = await categoryRepository.Where(c => c.Name == request.Name).AnyAsync();
            if (isSameCategoryExist)
                return ServiceResult<CreateCategoryResponse>.Fail("A category with the same name already exists.");

            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();

            var response = new CreateCategoryResponse(newCategory.Id);
            return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(response, $"/api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateCategoryRequest request) {
            var isCategoryNameExist = await categoryRepository
                .Where(c => c.Name == request.Name && c.Id != request.Id)
                .AnyAsync();
            if (isCategoryNameExist)
                return ServiceResult.Fail("A category with the same name already exists.");

            var category = await categoryRepository.GetByIdAsync(request.Id);

            category = mapper.Map(request, category);

            categoryRepository.Update(category!);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int categoryId) {
            var category = await categoryRepository.GetByIdAsync(categoryId);

            categoryRepository.Delete(category!);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}