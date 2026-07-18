using ProductCatalog.Web.Data.IService;
using ProductCatalog.Web.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Web.Data.Service
{
    public class ProductFilterService : IProductFilterService
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> products, string? searchText, string? priceUnit)
        {
            if (products is null)
            {
                return Enumerable.Empty<Product>();
            }

            var query = products.Where(p => p is not null);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var term = searchText.Trim();
                query = query.Where(p =>
                    (!string.IsNullOrEmpty(p.Title) && p.Title.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(p.Summary) && p.Summary.Contains(term, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(priceUnit))
            {
                query = query.Where(p => string.Equals(p.Title, priceUnit, StringComparison.OrdinalIgnoreCase));
            }

            return query;
        }

        public IReadOnlyList<string> GetAvailablePriceUnits(IEnumerable<Product> products)
        {
            if (products is null)
            {
                return Array.Empty<string>();
            }

            return products
                .Where(p => p is not null && !string.IsNullOrWhiteSpace(p.Title))
                .Select(p => p.Title!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(u => u, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
