using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services.Orders
{
    public interface IOrderApiClient
    {
        Task<List<Order>> GetInProgressOrdersAsync();

    }
}
