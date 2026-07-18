using ProductCatalog.Web.Utility.Model;

namespace ProductCatalog.Web.Data.IService
{
    public interface IProductApiService
    {
        Task<List<Product>> GetProductAsync();
        Task<ProductDetail?> GetProductDetailsAsync(int id);
        Task<Metrics?> GetProductMetricsAsync();
    }
}
