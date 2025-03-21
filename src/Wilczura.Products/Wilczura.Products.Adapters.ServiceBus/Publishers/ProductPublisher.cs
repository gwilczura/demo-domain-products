using MassTransit;
using Wilczura.Products.Contract.Models;
using Wilczura.Products.Ports.Models;
using Wilczura.Products.Ports.Publishers;

namespace Wilczura.Products.Adapters.ServiceBus.Publishers;

public class ProductPublisher : IProductPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishProductChangedAsync(ProductModel product)
    {
        await _publishEndpoint.Publish(new ProductChanged
        {
            ProductId = product.ProductId,
        });
    }
}
