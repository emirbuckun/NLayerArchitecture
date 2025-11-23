using System.Net;
using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products {
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService {
        public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count) {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productResponse = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<List<ProductResponse>>> GetAllListAsync() {
            var products = await productRepository.GetAll().ToListAsync();

            var productResponse = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int page, int pageSize) {
            var products = await productRepository.GetAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var productResponse = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<ProductResponse?>> GetByIdAsync(int productId) {
            var product = await productRepository.GetByIdAsync(productId);
            if (product is null) {
                return ServiceResult<ProductResponse?>.Fail("Product not found", HttpStatusCode.NotFound)!;
            }

            var productResponse = new ProductResponse(product.Id, product.Name, product.Price, product.Stock);

            return ServiceResult<ProductResponse?>.Success(productResponse);
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request) {
            var anyProductWithSameName = await productRepository.Where(p => p.Name == request.Name).AnyAsync();
            if (anyProductWithSameName) {
                return ServiceResult<CreateProductResponse>.Fail("A product with the same name already exists.");
            }

            var product = new Product { Name = request.Name, Price = request.Price, Stock = request.Stock };

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            var response = new CreateProductResponse(product.Id.ToString());

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(response, $"/api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request) {
            var product = await productRepository.GetByIdAsync((request.Id));
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

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request) {
            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product is null) {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Stock = request.NewStock;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int productId) {
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