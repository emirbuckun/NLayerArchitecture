using System.Net;
using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.ServiceBus;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Response;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using App.Domain.Events;
using AutoMapper;

namespace App.Application.Features.Products {
    public class ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork, ICacheService cacheService, IServiceBus serviceBus) : IProductService {
        private const string CacheKeyAllProducts = "all_products";

        public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count) {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productResponse = mapper.Map<List<ProductResponse>>(products);

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<List<ProductResponse>>> GetAllListAsync() {
            var products = await productRepository.GetAllAsync();

            var productResponse = mapper.Map<List<ProductResponse>>(products);

            return ServiceResult<List<ProductResponse>>.Success(productResponse);
        }

        public async Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int page, int pageSize) {
            // cache aside design pattern
            var cachedProducts = await cacheService.GetAsync<List<ProductResponse>>(CacheKeyAllProducts);
            if (cachedProducts is not null) {
                return ServiceResult<List<ProductResponse>>.Success(cachedProducts);
            }

            var products = await productRepository.GetAllPagedAsync(page, pageSize);

            var productResponse = mapper.Map<List<ProductResponse>>(products);
            await cacheService.SetAsync(CacheKeyAllProducts, productResponse, TimeSpan.FromMinutes(5));

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
            var isProductNmaeExist = await productRepository.AnyAsync(p => p.Name == request.Name);
            if (isProductNmaeExist) {
                return ServiceResult<CreateProductResponse>.Fail("A product with the same name already exists.");
            }

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            await serviceBus.PublishAsync<ProductCreatedEvent>(new ProductCreatedEvent(product.Id, product.Name, product.Price));

            var response = new CreateProductResponse(product.Id);

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(response, $"/api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request) {
            var isProductNameExist = await productRepository.AnyAsync(p => p.Name == request.Name && p.Id != request.Id);
            if (isProductNameExist) {
                return ServiceResult.Fail("A product with the same name already exists.");
            }

            var product = await productRepository.GetByIdAsync((request.Id));

            product = mapper.Map(request, product);

            productRepository.Update(product!);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request) {
            var product = await productRepository.GetByIdAsync(request.Id);

            product!.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int productId) {
            var product = await productRepository.GetByIdAsync(productId);

            productRepository.Delete(product!);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}