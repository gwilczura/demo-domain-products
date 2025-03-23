namespace Wilczura.Products.Host.Tests.Integration;

public static class HealthControllerTests
{
    public class TheGetMethod : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public TheGetMethod(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task WhenCalledReturnsValue()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/health/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/plain; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}