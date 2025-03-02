using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services
{
    public interface IChannelEngineApiClient
    {
        Task<List<Order>> GetInProgressOrdersAsync();
        Task UpdateProductStockAsync(string productId, int stock);
    }
}
