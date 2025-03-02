using ChannelEngine.Core.Helpers;
using ChannelEngine.Core.Services;
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
services.AddHttpClient<IChannelEngineApiClient, ChannelEngineApiClient>();
services.AddSingleton<ChannelEngineService>();

var serviceProvider = services.BuildServiceProvider();

// Resolve and run the service
var channelEngineService = serviceProvider.GetRequiredService<ChannelEngineService>();
var topProducts = await channelEngineService.GetTop5ProductsAsync();

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

    // Update new stock using merchantProductNo  
    await channelEngineService.UpdateStockForProductAsync(productToUpdate.MerchantProductNo, newStock);
    Console.WriteLine($"Updated stock for {productToUpdate.Name} to {newStock}.");
}