namespace App.Services.Products {
    public interface IProductService {
        public Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count);
        public Task<ServiceResult<List<ProductResponse>>> GetAllListAsync();
        public Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int page, int pageSize);
        public Task<ServiceResult<ProductResponse?>> GetByIdAsync(int productId);
        public Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
        public Task<ServiceResult> UpdateAsync(UpdateProductRequest request);
        public Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request);
        public Task<ServiceResult> DeleteAsync(int productId);
    }
}