using AutoFixture.Xunit2;
using Moq;
using Wilczura.Products.Application.Services;
using Wilczura.Products.Ports.Models;
using Wilczura.Products.Ports.Publishers;
using Wilczura.Products.Ports.Repositories;

namespace Wilczura.Products.Application.Tests.Unit.Services;

public static class ProductServiceTests
{
    public class UpsertAsyncMethod
    {
        [Theory, AutoData]
        public async Task When_UpsertExecutedSucessfullty_Then_EventPublished_Async(ProductModel model)
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var publisher = new Mock<IProductPublisher>();

            var service = new ProductService(repository.Object, publisher.Object);

            repository.Setup(a => a.UpsertAsync(It.IsAny<ProductModel>()))
                .ReturnsAsync([model]);

            // Act

            var result = await service.UpsertAsync(model);

            // Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var resultModel = result.First();
            Assert.NotNull(resultModel);
            Assert.True(resultModel.ProductId == model.ProductId);

            publisher.Verify(x => x.PublishProductChangedAsync(It.IsAny<ProductModel>()), Times.Once());
        }
    }
}