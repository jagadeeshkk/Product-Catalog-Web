using ProductCatalog.Data.IService;
using ProductCatalog.Web.Utility.Model;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace ProductCatalog.Data.Service
{
    public class ProductApiService: IProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>
            (
                "https://localhost:7112/api/v1/products"
            ) ?? [];
        }

        public async Task<ProductDetail?> GetProductDetailsAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProductDetail>
            (
                $"https://localhost:7112/api/v1/products/{id}"
            );
        }
        public async Task<Metrics?> GetProductMetricsAsync()
        {
            return await _httpClient.GetFromJsonAsync<Metrics>
            (
                "https://localhost:7112/api/v1/products/metrics"
            );
        }
    }
}
