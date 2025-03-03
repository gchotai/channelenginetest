namespace ChannelEngine.Core.Services.Products
{
    public interface IProductApiClient
    {
        Task UpdateProductStockAsync(string productId, int newStock);
    }
}
