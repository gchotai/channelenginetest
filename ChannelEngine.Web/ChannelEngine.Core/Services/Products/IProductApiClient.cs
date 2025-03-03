namespace ChannelEngine.Core.Services.Products
{
    public interface IProductApiClient
    {
        Task<bool> UpdateProductStockAsync(string productId, int newStock);
    }
}
