using App.Repositories.Products;

namespace App.Services.Products {
    public class ProductService(IProductRepository productRepository) : IProductService {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<List<Product>> GetTopPriceProductsAsync(int count) {
            return await _productRepository.GetTopPriceProductsAsync(count);
        }
    }
}