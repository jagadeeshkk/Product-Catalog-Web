using ProductCatalog.Web.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Data.IService
{
    public interface IProductApiService
    {
        Task<List<Product>> GetProductAsync();
        Task<ProductDetail?> GetProductDetailsAsync(int id);
        Task<Metrics?> GetProductMetricsAsync();
    }
}
