using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Wilczura.Common.Host.Extensions;
using Wilczura.Common.Host.Models;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Products.Adapters.AspNet.Controllers;
using Wilczura.Products.Adapters.Postgres.Extensions;
using Wilczura.Products.Adapters.ServiceBus.Extensions;
using Wilczura.Products.Host.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configName = "Products";
// Add services to the container.
var logger = builder.GetStartupLogger();
builder.AddCustomHostServices(
    configName,
    DefaultListenerName,
    AuthenticationType.Default,
    new AssemblyPart(typeof(HealthController).Assembly),
    logger);
builder.AddProductsApplication();
builder.AddProductsPostgres(string.Empty, logger);
builder.AddProductsServiceBus(string.Empty);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseObservabilityDefaults();

app.Run();

// needed for integration tests
public partial class Program
{
    public const string DefaultListenerName = "WilczuraObservability";
}
