using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderApiClient _apiClient;

        public OrderService(IOrderApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<Product>> GetTop5ProductsAsync()
        {
            // Fetch orders from the API client
            var orders = await _apiClient.GetInProgressOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return new List<Product>(); // Return empty list instead of null
            }

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

    }
}
