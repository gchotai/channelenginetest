namespace ChannelEngine.Core.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductApiClient _apiClient;

        public ProductService(IProductApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> UpdateStockForProductAsync(string merchantProductNo, int newStock)
        {
            return await _apiClient.UpdateProductStockAsync(merchantProductNo, newStock);
        }
    }
}
