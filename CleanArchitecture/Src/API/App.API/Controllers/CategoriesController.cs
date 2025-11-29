using App.API.Filters;
using App.Application.Features.Categories;
using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Update;
using App.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers {
    public class CategoriesController(ICategoryService categoryService) : CustomBaseController {
        [HttpGet("with-products/{id:int}")]
        public async Task<IActionResult> GetCategoryWithProducts(int id) =>
            CreateActionResult(await categoryService.GetCategoryWithProductsAsync(id));

        [HttpGet("with-products")]
        public async Task<IActionResult> GetCategoryWithProducts() =>
            CreateActionResult(await categoryService.GetCategoryWithProductsAsync());

        [HttpGet]
        public async Task<IActionResult> GetAllCategories() =>
            CreateActionResult(await categoryService.GetAllListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id) =>
            CreateActionResult(await categoryService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request) =>
            CreateActionResult(await categoryService.CreateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Category, int>))]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request) =>
            CreateActionResult(await categoryService.UpdateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Category, int>))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id) =>
            CreateActionResult(await categoryService.DeleteAsync(id));
    }
}