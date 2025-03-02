using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services
{
    public class ChannelEngineService
    {
        private readonly IChannelEngineApiClient _apiClient;

        public ChannelEngineService(IChannelEngineApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<Product>> GetTop5ProductsAsync()
        {
            var orders = await _apiClient.GetInProgressOrdersAsync();
            var products = orders
                .SelectMany(o => o.Lines)
                .GroupBy(l => new { l.Gtin, l.Description, l.MerchantProductNo })
                .Select(g => new Product
                {
                    Name = g.Key.Description,
                    Gtin = g.Key.Gtin,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    MerchantProductNo = g.Key.MerchantProductNo
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(5)
                .ToList();

            return products;
        }

        public async Task<bool> UpdateStockForProductAsync(string merchantProductNo, int newStock)
        {
            return await _apiClient.UpdateProductStockAsync(merchantProductNo, newStock);
        }
    }
}
