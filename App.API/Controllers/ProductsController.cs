using App.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers {
    public class ProductsController(IProductService productService) : CustomBaseController {
        [HttpGet("top-price/{count}")]
        public async Task<IActionResult> GetTopPriceProducts(int count) =>
            CreateActionResult(await productService.GetTopPriceProductsAsync(count));

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() =>
            CreateActionResult(await productService.GetAllList());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id) =>
            CreateActionResult(await productService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request) =>
            CreateActionResult(await productService.CreateAsync(request));

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request) =>
            CreateActionResult(await productService.UpdateAsync(request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) =>
            CreateActionResult(await productService.DeleteAsync(id));
    }
}