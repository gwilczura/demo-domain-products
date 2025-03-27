using NetArchTest.Rules;
using Shouldly;

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
        public void When_TypesAreCustom_Then_TheyShouldExist()
        {
            var types = Types.InAssemblies(_assemblyFixture.HostAssemblies);
            var domainTypes = types.That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix);
            var domainTypesList = domainTypes.GetTypes().ToArray();

            Assert.InRange(domainTypesList.Length, 1, int.MaxValue);
        }

        [Fact]
        public void When_TypeIsFromHost_Then_NoOtherNamespaceDependsOnIt()
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

        [Fact]
        public void When_TypeIsFromContract_Then_ItShouldExist()
        {
            var contractNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Contract";
            var types = Types.InAssemblies(_assemblyFixture.ContractAssemblies)
                .That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix)
                .GetTypes();

            (types).ShouldNotBeEmpty("There Should be types in contract");
        }

        [Fact]
        public void When_TypeIsFromContract_Then_ItDoesntDependOnOtherAssemblies()
        {
            var contractNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Contract";
            var result = Types.InAssemblies(_assemblyFixture.ContractAssemblies)
                .That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix)
                .Should()
                .ResideInNamespaceStartingWith(contractNamespace)
                .GetResult();

            (result.FailingTypeNames ?? []).ShouldBeEmpty("There Should be no types that contract depends on");
        }

        [Fact]
        public void When_TypeIsFromClient_Then_ItShouldExist()
        {
            var contractNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Client";
            var types = Types.InAssemblies(_assemblyFixture.ClientAssemblies)
                .That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix)
                .GetTypes();

            (types).ShouldNotBeEmpty("There Should be types in client assemblies");
        }

        [Fact]
        public void When_TypeIsFromClient_Then_ItShouldDependOnlyOnContract()
        {
            var clientNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Client";
            var contractNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Contract";
            var result = Types.InAssemblies(_assemblyFixture.ContractAssemblies)
                .That()
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix)
                .Should()
                .ResideInNamespaceStartingWith(clientNamespace)
                .Or()
                .ResideInNamespaceStartingWith(contractNamespace)
                .GetResult();

            (result.FailingTypeNames ?? []).ShouldBeEmpty("There Should be no types other than contract that client depends on");
        }

        [Fact]
        public void When_TypeIsFromAdapter_Then_OnlyHostDependsOnIt()
        {
            var hostNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Host";
            var adaptersNamespace = $"{_assemblyFixture.DomainNamespacePrefix}.Adapters";

            var adapterAssemblies = AssemblyHelper.GetByPrefix(_assemblyFixture.HostAssemblies, adaptersNamespace);
            List<string> failures = new List<string>();
            //check for each adapter assembly
            foreach (var adapterAssembly in adapterAssemblies)
            {
                var assemblyName = adapterAssembly.GetName().Name!;
                Assert.NotNull(assemblyName);
                Console.WriteLine(assemblyName);

                var typesNotFromHostAndCurrentAdapter = Types.InAssemblies(_assemblyFixture.HostAssemblies)
                .That()
                // fiter only "Wilczura.Products" types
                .ResideInNamespaceStartingWith(_assemblyFixture.DomainNamespacePrefix)
                .And()
                // assuming namespace match assembly name
                .DoNotResideInNamespaceStartingWith(assemblyName);

                var adapterAssemblyTypes = Types.InAssembly(adapterAssembly);
                var adapterAssemblyTypesNames = adapterAssemblyTypes.GetTypes().Select(a=>a.FullName).ToArray();

                var result = typesNotFromHostAndCurrentAdapter
                    .ShouldNot()
                    .HaveDependencyOnAny(adapterAssemblyTypesNames).
                    GetResult();

                failures.AddRange((result.FailingTypeNames ?? []).Select(t => $"{t} - {assemblyName}").ToArray());
            }

            failures.ShouldNotBeEmpty("There should be dependencies on the adapters");

            failures.Where(a => !a.StartsWith(hostNamespace)).ShouldBeEmpty("Dependencies should only be in the host");
        }
    }
}