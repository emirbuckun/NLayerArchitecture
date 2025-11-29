using App.Repositories.Products;
using App.Services.Filters;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers {
    public class ProductsController(IProductService productService) : CustomBaseController {
        [HttpGet("top-price/{count:int}")]
        public async Task<IActionResult> GetTopPriceProducts(int count) =>
            CreateActionResult(await productService.GetTopPriceProductsAsync(count));

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() =>
            CreateActionResult(await productService.GetAllListAsync());

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAllProducts(int page, int pageSize) =>
            CreateActionResult(await productService.GetPagedAllListAsync(page, pageSize));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id) =>
            CreateActionResult(await productService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request) =>
            CreateActionResult(await productService.CreateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request) =>
            CreateActionResult(await productService.UpdateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateProductStock([FromBody] UpdateProductStockRequest request) =>
            CreateActionResult(await productService.UpdateStockAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id) =>
            CreateActionResult(await productService.DeleteAsync(id));
    }
}