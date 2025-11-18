using App.Repositories;
using App.Repositories.Products;

namespace App.Services {
    public class ProductService(IProductRepository productRepository) {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<List<Product>> GetTopPriceProductsAsync(int count) {
            return await _productRepository.GetTopPriceProductsAsync(count);
        }
    }
}