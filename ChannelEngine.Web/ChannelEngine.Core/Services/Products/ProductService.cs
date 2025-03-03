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
            if (string.IsNullOrWhiteSpace(merchantProductNo))
            {
                throw new ArgumentException("Merchant product number cannot be null or empty.", nameof(merchantProductNo));
            }

            if (newStock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newStock), "Stock quantity cannot be negative.");
            }

            return await _apiClient.UpdateProductStockAsync(merchantProductNo, newStock);
        }
    }
}
