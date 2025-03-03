using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services.Orders
{
    public interface IOrderService
    {
        Task<List<Product>> GetTop5ProductsAsync();

    }
}
