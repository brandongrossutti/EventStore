﻿using System.Reflection;
using GHI.Domain.Mapping;
using GHI.WireUp;

namespace GHI.Bus.InMemory
{
    public class Initializer : IInitializer
    {
        public WireUpItem GetWireUp(InitializerWireUp wireup)
        {
            return new WireUpItem(
                x =>
                    {
                        x.For<IHandlerResolver>()
                            .Singleton()
                            .Use<AggregateRootInspector>();

                        x.Scan(
                            s =>
                                {
                                    foreach (Assembly assembly in wireup.Assemblies)
                                    {
                                        s.Assembly(assembly);
                                        s.Convention<MessageHandlerTypeConvention>();
                                        s.Convention<RequestHandlerTypeConvention>();
                                    }
                                    s.WithDefaultConventions();
                                });
                    }
                );
        }
    }
}
