using Wilczura.Products.Ports.Models;

namespace Wilczura.Products.Ports.Services;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> GetAsync(long? productId);
    Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model);
}
