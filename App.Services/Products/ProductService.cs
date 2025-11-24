using System.Net;
using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products {
    public class ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork) : IProductService {
        public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count) {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productResponse = mapper.Map<List<ProductResponse>>(products);

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<List<ProductResponse>>> GetAllListAsync() {
            var products = await productRepository.GetAll().ToListAsync();

            var productResponse = mapper.Map<List<ProductResponse>>(products);

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int page, int pageSize) {
            var products = await productRepository.GetAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productResponse = mapper.Map<List<ProductResponse>>(products);

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<ProductResponse?>> GetByIdAsync(int productId) {
            var product = await productRepository.GetByIdAsync(productId);
            if (product is null) {
                return ServiceResult<ProductResponse?>.Fail("Product not found", HttpStatusCode.NotFound)!;
            }

            var productResponse = mapper.Map<ProductResponse>(product);

            return ServiceResult<ProductResponse?>.Success(productResponse);
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request) {
            var isProductNmaeExist = await productRepository.Where(p => p.Name == request.Name).AnyAsync();
            if (isProductNmaeExist) {
                return ServiceResult<CreateProductResponse>.Fail("A product with the same name already exists.");
            }

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            var response = new CreateProductResponse(product.Id);

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(response, $"/api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request) {
            var product = await productRepository.GetByIdAsync((request.Id));
            if (product is null) {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            var isProductNmaeExist = await productRepository.Where(p => p.Name == request.Name && p.Id != request.Id).AnyAsync();
            if (isProductNmaeExist) {
                return ServiceResult.Fail("A product with the same name already exists.");
            }

            product = mapper.Map(request, product);

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