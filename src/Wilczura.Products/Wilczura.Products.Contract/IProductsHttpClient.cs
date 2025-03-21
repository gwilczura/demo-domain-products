using Wilczura.Products.Contract.Models;

namespace Wilczura.Products.Contract;

public interface IProductsHttpClient
{
    public Task<IEnumerable<ProductDto>> GetProductsAsync(long? id, CancellationToken cancellationToken);
    public Task<IEnumerable<ProductDto>> UpsertProductAsync(ProductDto dto, CancellationToken cancellationToken);
}
