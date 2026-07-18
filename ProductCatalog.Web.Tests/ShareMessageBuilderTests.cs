using ProductCatalog.Web.Data.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Web.Tests
{
    [TestFixture]
    public class ShareMessageBuilderTests
    {
        private ShareMessageBuilder _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new ShareMessageBuilder();
        }

        [Test]
        public void BuildAddToListMessage_WithCity_MatchesSpecFormat()
        {
            var result = _sut.BuildAddToListMessage("Bananas", "$0.59/lb", "Chicago");

            Assert.That(result, Is.EqualTo("Bananas - $0.59/lb from Chicago added to list"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void BuildAddToListMessage_MissingCity_FallsBackToUnknownLocation(string? city)
        {
            var result = _sut.BuildAddToListMessage("Bananas", "$0.59/lb", city);

            Assert.That(result, Is.EqualTo("Bananas - $0.59/lb from an unknown location added to list"));
        }

        [Test]
        public void BuildAddToListMessage_TrimsWhitespaceInInputs()
        {
            var result = _sut.BuildAddToListMessage("  Bananas  ", "  $0.59/lb  ", "  Chicago  ");

            Assert.That(result, Is.EqualTo("Bananas - $0.59/lb from Chicago added to list"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void BuildAddToListMessage_MissingTitle_Throws(string? title)
        {
            Assert.Throws<ArgumentException>(() => _sut.BuildAddToListMessage(title!, "$0.59/lb", "Chicago"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void BuildAddToListMessage_MissingPrice_Throws(string? price)
        {
            Assert.Throws<ArgumentException>(() => _sut.BuildAddToListMessage("Bananas", price!, "Chicago"));
        }
    }
}
