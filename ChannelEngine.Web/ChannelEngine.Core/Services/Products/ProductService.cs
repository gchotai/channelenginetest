namespace ChannelEngine.Core.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductApiClient _apiClient;

        public ProductService(IProductApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task UpdateStockForProductAsync(string merchantProductNo, int newStock)
        {
            await _apiClient.UpdateProductStockAsync(merchantProductNo, newStock);
        }
    }
}
