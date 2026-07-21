using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Net;

namespace ProductCatalog.Web.Tests
{
    [TestFixture]
    public class ProductApiServiceTests
    {
        private static Mock<HttpMessageHandler> CreateHandlerMock(
            HttpStatusCode statusCode, string? jsonContent = null)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(statusCode)
                {
                    Content = jsonContent is null
                        ? new StringContent(string.Empty)
                        : new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
                });
            return handlerMock;
        }

        private static Mock<HttpMessageHandler> CreateThrowingHandlerMock(Exception exception)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(exception);
            return handlerMock;
        }

        private static ProductApiService CreateService(HttpMessageHandler handler)
        {
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7112/")
            };
            return new ProductApiService(httpClient, NullLogger<ProductApiService>.Instance);
        }

        [Test]
        public async Task GetProductAsync_SuccessResponse_ReturnsDeserializedList()
        {
            const string json = """
                [
                  { "id": 0, "title": "Bananas", "summary": "Fresh bananas.", "imageUrl": "https://x/b.png" },
                  { "id": 1, "title": "Gala Apples", "summary": "Crisp apples.", "imageUrl": "https://x/a.png" }
                ]
                """;
            var service = CreateService(CreateHandlerMock(HttpStatusCode.OK, json).Object);

            var result = await service.GetProductAsync();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Title, Is.EqualTo("Bananas"));
        }

        [Test]
        public async Task GetProductAsync_NonSuccessStatus_ReturnsEmptyList()
        {
            var service = CreateService(CreateHandlerMock(HttpStatusCode.InternalServerError).Object);

            var result = await service.GetProductAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetProductAsync_HttpRequestException_ReturnsEmptyListInsteadOfThrowing()
        {
            var service = CreateService(CreateThrowingHandlerMock(new HttpRequestException("network down")).Object);

            var result = await service.GetProductAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetProductDetailsAsync_NotFound_ReturnsNull()
        {
            var service = CreateService(CreateHandlerMock(HttpStatusCode.NotFound).Object);

            var result = await service.GetProductDetailsAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetProductDetailsAsync_Success_ReturnsDetail()
        {
            const string json = """
                {
                  "id": 0, "title": "Bananas", "summary": "Fresh bananas.",
                  "description": "Rich in potassium.", "price": "$0.59/lb",
                  "imageUrl": "https://x/b.png"
                }
                """;
            var service = CreateService(CreateHandlerMock(HttpStatusCode.OK, json).Object);

            var result = await service.GetProductDetailsAsync(0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Price, Is.EqualTo("$0.59/lb"));
        }

        [Test]
        public void GetProductDetailsAsync_NegativeId_Throws()
        {
            var service = CreateService(CreateHandlerMock(HttpStatusCode.OK).Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetProductDetailsAsync(-1));
        }

        [Test]
        public async Task GetProductMetricsAsync_NonSuccessStatus_ReturnsNull()
        {
            var service = CreateService(CreateHandlerMock(HttpStatusCode.BadGateway).Object);

            var result = await service.GetProductMetricsAsync();

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetProductMetricsAsync_Success_ReturnsMetrics()
        {
            const string json = """
                {
                  "totalProducts": 2, "averagePrice": 1.5,
                  "mostExpensive": { "id": 1, "title": "Gala Apples", "price": "$2.00/each" },
                  "leastExpensive": { "id": 0, "title": "Bananas", "price": "$0.59/lb" },
                  "byPriceUnit": { "/lb": 1, "/each": 1, "/bunch": 0, "/head": 0, "/bag": 0 }
                }
                """;
            var service = CreateService(CreateHandlerMock(HttpStatusCode.OK, json).Object);

            var result = await service.GetProductMetricsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.TotalProducts, Is.EqualTo(2));
            Assert.That(result.MostExpensiveProduct.Title, Is.EqualTo("Gala Apples"));
        }
    }
}
