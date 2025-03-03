namespace ChannelEngine.Core.Services.Products
{
    public interface IProductService
    {
        Task UpdateStockForProductAsync(string merchantProductNo, int newStock);
    }
}
