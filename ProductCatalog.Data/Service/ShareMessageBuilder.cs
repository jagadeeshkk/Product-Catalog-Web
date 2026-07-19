using ProductCatalog.Web.Business.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Web.Business.Service
{
    public class ShareMessageBuilder : IShareMessageBuilder
    {
        public string BuildAddToListMessage(string title, string price, string? city)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Product title is required.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(price))
            {
                throw new ArgumentException("Product price is required.", nameof(price));
            }

            var resolvedCity = string.IsNullOrWhiteSpace(city) ? "an unknown location" : city.Trim();

            return $"{title.Trim()} - {price.Trim()} from {resolvedCity} added to list";
        }
    }
}
