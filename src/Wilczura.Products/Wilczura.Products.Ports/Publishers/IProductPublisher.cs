using Wilczura.Products.Ports.Models;

namespace Wilczura.Products.Ports.Publishers;

public interface IProductPublisher
{
    Task PublishProductChangedAsync(ProductModel product);
}
