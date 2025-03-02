using ChannelEngine.Core.Helpers;
using ChannelEngine.Core.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ChannelEngine.Core.Services
{
    public class ChannelEngineApiClient : IChannelEngineApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ChannelEngineSettings _settings;

        public ChannelEngineApiClient(HttpClient httpClient, IOptions<ChannelEngineSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value; // Access the configured settings
        }

        public async Task<List<Order>> GetInProgressOrdersAsync()
        {
            var url = $"{_settings.BaseUrl}v2/orders?statuses=IN_PROGRESS&apikey={_settings.ApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<Order>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Content ?? new List<Order>();
            }

            throw new HttpRequestException($"Failed to fetch orders. Status code: {response.StatusCode}");
        }

        public async Task<bool> UpdateProductStockAsync(string merchantProductNo, int newStock)
        {
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
                throw new HttpRequestException($"Failed to update stock. Status code: {response.StatusCode}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
