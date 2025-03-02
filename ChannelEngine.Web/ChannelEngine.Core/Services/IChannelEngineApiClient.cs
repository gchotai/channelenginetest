using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services
{
    public interface IChannelEngineApiClient
    {
        Task<List<Order>> GetInProgressOrdersAsync();
        Task<bool> UpdateProductStockAsync(string productId, int newStock);
    }
}
