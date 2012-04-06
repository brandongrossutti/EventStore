using System;
using System.Reflection;

namespace GHI.Commons.Extensions
{
    public static class AssemblyExtensions
    {
        public static void LoadAllDependencies(this Assembly assembly, Func<AssemblyName, bool> dependentAssemblyMatcher)
        {
            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                if (dependentAssemblyMatcher(assemblyName))
                {
                    Assembly referencedAssembly = Assembly.Load(assemblyName);
                    referencedAssembly.LoadAllDependencies(dependentAssemblyMatcher);
                }
            }
        }

        public static void LoadAllDependencies(this Assembly assembly)
        {
            assembly.LoadAllDependencies(x => true);
        }
    }
}
