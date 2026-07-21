using ProductCatalog.Web.Utility.Model;

namespace ProductCatalog.Web.Business.IService
{
    public interface IProductFilterService
    {
        IEnumerable<Product> Filter(IEnumerable<Product> products, string? searchText, string? priceUnit);
        IReadOnlyList<string> GetAvailablePriceUnits(IEnumerable<Product> products);
    }
}

