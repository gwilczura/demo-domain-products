using NetArchTest.Rules;

namespace Wilczura.Products.Tests.Architecture;

public static class ArchitectureTests
{
        public class Dependencies : IClassFixture<AssemblyFixture>
    {
        private readonly AssemblyFixture _assemblyFixture;

        public Dependencies(AssemblyFixture assemblyFixture)
        {
            _assemblyFixture = assemblyFixture;
        }

        [Fact]
        public void Whan_TypesAreCustom_Then_TheyShouldExist()
        {
            var types = Types.InAssemblies(_assemblyFixture.HostAssemblies);
            var domainTypes = types.That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix);
            var domainTypesList = domainTypes.GetTypes().ToArray();

            Assert.InRange(domainTypesList.Length, 1, int.MaxValue);
        }

        [Fact]
        public void Whan_TypeIsFromHost_Then_NoOtherNamespaceDependsOnIt()
        {
            var hostNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Host";
            var domainTypes = Types.InAssemblies(_assemblyFixture.HostAssemblies)
                .That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix)
                .And()
                .DoNotResideInNamespaceStartingWith(hostNamespace);
            var testResult = domainTypes.ShouldNot()
                .HaveDependencyOn(hostNamespace)
                .GetResult();

            Assert.Empty(testResult.FailingTypeNames ?? []);
        }
    }
}