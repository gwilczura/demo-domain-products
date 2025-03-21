using Microsoft.EntityFrameworkCore;
using Wilczura.Products.Adapters.Postgres.Models;

namespace Wilczura.Products.Adapters.Postgres;

public class ProductsContext(DbContextOptions<ProductsContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
