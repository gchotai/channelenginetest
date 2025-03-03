namespace ChannelEngine.Core.Services.Products
{
    public interface IProductService
    {
        Task<bool> UpdateStockForProductAsync(string merchantProductNo, int newStock);
    }
}
