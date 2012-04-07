using System.Collections.Generic;
using System.Reflection;

namespace GHI.WireUp
{
    public class InitializerWireUpBuilder
    {
        
        private bool RunDefaults { get; set; }
        private string AssemblyPrefix { get; set; }
        private List<AssemblyName> AssembliesNotReferencedToLoad { get; set; }

        public InitializerWireUpBuilder ShouldRunDefault(bool runDefaults)
        {
            RunDefaults = runDefaults;
            return this;
        }

        public InitializerWireUpBuilder WithAssemblyNotReferencedToLoad(string assembly)
        {
            if(AssembliesNotReferencedToLoad==null)
            {AssembliesNotReferencedToLoad = new List<AssemblyName>();}
            AssemblyName name = new AssemblyName(assembly);
            AssembliesNotReferencedToLoad.Add(name);
            return this;
        }

        public InitializerWireUpBuilder LoadAssemblyPrefix(string prefix)
        {
            AssemblyPrefix = prefix;
            return this;
        }


        public static implicit operator InitializerWireUp(InitializerWireUpBuilder builder)
        {return new InitializerWireUp(builder.AssemblyPrefix, builder.RunDefaults, builder.AssembliesNotReferencedToLoad);}
    }
}
