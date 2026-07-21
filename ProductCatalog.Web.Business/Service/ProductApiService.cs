using Microsoft.Extensions.Logging;
using ProductCatalog.Web.Business.IService;
using ProductCatalog.Web.Utility.Model;
using System.Net;
using System.Net.Http.Json;

namespace ProductCatalog.Web.Business.Service
{
    public class ProductApiService: IProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductApiService> _logger;

        public ProductApiService(HttpClient httpClient, ILogger<ProductApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Product>> GetProductAsync()
        {
            const string requestUri = "api/v1/products";
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Product list request to {RequestUri} failed with status {StatusCode}.",
                        requestUri, response.StatusCode);
                    return new List<Product>();
                }

                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                return products ?? new List<Product>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving product list from {RequestUri}.", requestUri);
                return new List<Product>();
            }
        }

        public async Task<ProductDetail?> GetProductDetailsAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Product id cannot be negative.");
            }

            var requestUri = $"api/v1/products/{id}";
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Product {ProductId} was not found.", id);
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Product detail request for id {ProductId} failed with status {StatusCode}.",
                        id, response.StatusCode);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<ProductDetail>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving product detail for id {ProductId}.", id);
                return null;
            }
        }
        public async Task<Metrics?> GetProductMetricsAsync()
        {
            const string requestUri = "api/v1/products/metrics";
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Metrics request to {RequestUri} failed with status {StatusCode}.",
                        requestUri, response.StatusCode);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<Metrics>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving product metrics from {RequestUri}.", requestUri);
                return null;
            }
        }
    }
}
