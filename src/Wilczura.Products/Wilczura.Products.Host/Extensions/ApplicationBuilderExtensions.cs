using Wilczura.Products.Adapters.Postgres.Repositories;
using Wilczura.Products.Adapters.ServiceBus.Publishers;
using Wilczura.Products.Application.Services;
using Wilczura.Products.Ports.Publishers;
using Wilczura.Products.Ports.Repositories;
using Wilczura.Products.Ports.Services;

namespace Wilczura.Products.Host.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddProductsApplication(
        this IHostApplicationBuilder app)
    {
        app.Services.AddScoped<IProductRepository, ProductRepository>();
        app.Services.AddScoped<IProductService, ProductService>();
        app.Services.AddScoped<IProductPublisher, ProductPublisher>();
        return app;
    }
}
