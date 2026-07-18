using ProductCatalog.Web.Data.Service;
using ProductCatalog.Web.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Web.Tests
{
    [TestFixture]
    public class ProductFilterServiceTests
    {
        private static List<Product> SampleProducts() => new()
        {
            new Product { Id = 0, Title = "Bananas", Summary = "Fresh yellow bananas." },
            new Product { Id = 1, Title = "Gala Apples", Summary = "Crisp and sweet." },
            new Product { Id = 2, Title = "Garlic", Summary = "Pungent bananas pairing." },
            new Product { Id = 3, Title = "Carrots", Summary = "Great snack." },
        };

        private ProductFilterService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new ProductFilterService();
        }

        [Test]
        public void Filter_NoCriteria_ReturnsAllProducts()
        {
            var result = _sut.Filter(SampleProducts(), null, null).ToList();
            Assert.That(result, Has.Count.EqualTo(4));
        }

        [Test]
        public void Filter_BySearchText_AlsoMatchesSummary()
        {
            // "bananas" appears in Garlic's summary but not its title.
            var result = _sut.Filter(SampleProducts(), "bananas", null).ToList();
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Any(p => p.Title == "Garlic"), Is.True);
        }

        [Test]
        public void Filter_NullProducts_ReturnsEmpty()
        {
            var result = _sut.Filter(null!, "anything", null);
            Assert.That(result, Is.Empty);
        }
    }
}
