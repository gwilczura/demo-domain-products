using System.Reflection;
using Wilczura.Products.Client;

namespace Wilczura.Products.Tests.Architecture;

public class AssemblyFixture : IDisposable
{
    public IList<Assembly> HostAssemblies { get; set; }
    public IList<Assembly> ClientAssemblies { get; set; }

    public string DomainNamespacePrefix { get; set; }

    public AssemblyFixture()
    {
        HostAssemblies = AssemblyHelper.GetAllAssemblies(typeof(Program).Assembly).ToList();
        ClientAssemblies = AssemblyHelper.GetAllAssemblies(typeof(ProductsHttpClient).Assembly).ToList();
        DomainNamespacePrefix = "Wilczura.Products";
    }

    public void Dispose()
    {
    }
}
