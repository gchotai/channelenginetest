using ChannelEngine.Core.Helpers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ChannelEngine.Core.Services.Products
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ChannelEngineSettings _settings;

        public ProductApiClient(HttpClient httpClient, IOptions<ChannelEngineSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value; // Access the configured settings
        }

        public async Task<bool> UpdateProductStockAsync(string merchantProductNo, int newStock)
        {
            try
            {
                // Api call for update stock using merchantProductNo
                var url = $"{_settings.BaseUrl}v2/products/{merchantProductNo}?apikey={_settings.ApiKey}";

                // Create the JSON Patch request
                var patchDocument = new[]
                {
                    new
                    {
                        value = newStock,
                        path = "Stock",
                        op = "replace"
                    }
                };

                // Serialize the patch document to JSON
                var jsonPayload = JsonSerializer.Serialize(patchDocument);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json-patch+json");

                // Send the PATCH request
                var response = await _httpClient.PatchAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"API request failed with status {response.StatusCode}: {errorMessage}");
                }

                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to update stock for {merchantProductNo} due to a network or API error.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException($"Failed to serialize stock update payload for {merchantProductNo}.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An unexpected error occurred, Failed to update stock {merchantProductNo}.", ex);
            }
        }
    }
}
