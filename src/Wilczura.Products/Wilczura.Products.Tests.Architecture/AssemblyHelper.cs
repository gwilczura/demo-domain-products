using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wilczura.Products.Tests.Architecture;

public static class AssemblyHelper
{
    public static IEnumerable<Assembly> GetAllAssemblies(Assembly startAssembly)
    {
        var knownAssemblies = new HashSet<string>();
        var stack = new Stack<Assembly>();

        stack.Push(startAssembly);

        do
        {
            var currentAssembly = stack.Pop();

            yield return currentAssembly;

            foreach (var reference in currentAssembly.GetReferencedAssemblies())
            {
                if(knownAssemblies.Add(reference.FullName))
                {
                    stack.Push(Assembly.Load(reference));
                }
            }

        }
        while (stack.Count > 0);
    }

    public static IEnumerable<Assembly> GetByPrefix(IEnumerable<Assembly> assemblies, string prefix)
    {
        return assemblies.Where(a => (a.GetName().Name ?? string.Empty).ToLowerInvariant().StartsWith(prefix.ToLowerInvariant())).ToArray();
    }
}
