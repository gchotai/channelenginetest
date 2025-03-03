using ChannelEngine.Core.Helpers;
using ChannelEngine.Core.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ChannelEngine.Core.Services.Orders
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ChannelEngineSettings _settings;

        public OrderApiClient(HttpClient httpClient, IOptions<ChannelEngineSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value; // Access the configured settings
        }

        public async Task<List<Order>> GetInProgressOrdersAsync()
        {
            try
            {
                var url = $"{_settings.BaseUrl}v2/orders?statuses=IN_PROGRESS&apikey={_settings.ApiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"API request failed with status {response.StatusCode}: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<Order>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Content ?? new List<Order>();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to get orders due to a network or API error.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException($"Failed to serialize Json orders data.", ex);
            }
        }
    }
}
