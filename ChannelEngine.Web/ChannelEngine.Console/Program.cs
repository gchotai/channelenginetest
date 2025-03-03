using ChannelEngine.Core.Helpers;
using ChannelEngine.Core.Services.Orders;
using ChannelEngine.Core.Services.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

// Register configuration
services.Configure<ChannelEngineSettings>(configuration.GetSection("ChannelEngine"));

// Register HttpClient and services
services.AddHttpClient<IProductApiClient, ProductApiClient>();
services.AddScoped<IProductService, ProductService>();
services.AddHttpClient<IOrderApiClient, OrderApiClient>();
services.AddScoped<IOrderService, OrderService>();

var serviceProvider = services.BuildServiceProvider();

// Run the service
var orderService = serviceProvider.GetRequiredService<IOrderService>();
var topProducts = await orderService.GetTop5ProductsAsync();

Console.WriteLine("Top 5 Products:");
foreach (var product in topProducts)
{
    Console.WriteLine($"{product.Name} (GTIN: {product.Gtin}) - Quantity: {product.TotalQuantity}");
}

if (topProducts.Any())
{
    // Set updated stock number 
    int newStock = 25;
    var productToUpdate = topProducts.First();

    // Run the service
    var productService = serviceProvider.GetRequiredService<IProductService>();

    // Update new stock using merchantProductNo  
    await productService.UpdateStockForProductAsync(productToUpdate.MerchantProductNo, newStock);
    Console.WriteLine($"Stock has been updated for {productToUpdate.Name} to {newStock}.");
}