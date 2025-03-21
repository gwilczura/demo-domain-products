using Wilczura.Products.Ports.Models;

namespace Wilczura.Products.Ports.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductModel>> GetAsync(long? productId);
    Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model);
}
