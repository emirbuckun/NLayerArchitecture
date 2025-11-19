using System.Net;
using App.Repositories;
using App.Repositories.Products;

namespace App.Services.Products {
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService {
        public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count) {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productResponse = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return new ServiceResult<List<ProductResponse>> { Data = productResponse };
        }

        public async Task<ServiceResult<ProductResponse>> GetProductByIdAsync(int productId) {
            var product = await productRepository.GetByIdAsync(productId);
            if (product is null) {
                return ServiceResult<ProductResponse>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            var productResponse = new ProductResponse(product.Id, product.Name, product.Price, product.Stock);

            return ServiceResult<ProductResponse>.Success(productResponse);
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateProductAsync(CreateProductRequest request) {
            var newProduct = new Product { Name = request.Name, Price = request.Price, Stock = request.Stock };

            await productRepository.AddAsync(newProduct);
            await unitOfWork.SaveChangesAsync();

            var response = new CreateProductResponse(newProduct.Id.ToString());

            return ServiceResult<CreateProductResponse>.Success(response, HttpStatusCode.Created);
        }

        public async Task<ServiceResult> UpdateProductAsync(UpdateProductRequest request) {
            var product = await productRepository.GetByIdAsync(int.Parse(request.Id));
            if (product is null) {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteProductAsync(int productId) {
            var product = await productRepository.GetByIdAsync(productId);
            if (product is null) {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            productRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}