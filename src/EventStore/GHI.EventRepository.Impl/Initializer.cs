using System;
using EventStore;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository.Impl.UnitOfWork;
using GHI.WireUp;
using StructureMap;
using StructureMap.Attributes;

namespace GHI.EventRepository.Impl
{
    public class Initializer
    {
        public static WireUpItem GetWireUp()
        {
            IStoreEvents store = EventStore.Wireup.Init()
                .LogToOutputWindow()
                .UsingInMemoryPersistence() // Connection string is in app.config
                .EnlistInAmbientTransaction() // two-phase commit
                .InitializeStorageEngine()
                .Build();

            ObjectFactory.Inject<IStoreEvents>(store);

            return new WireUpItem(
                x =>
                    {
                        x.ForRequestedType<IUnitOfWorkFactory>()
                            .TheDefaultIsConcreteType<EventStoreUnitOfWorkFactory>()
                            .CacheBy(InstanceScope.Singleton);

                        x.ForRequestedType<IRepository<Guid>>()
                            .TheDefaultIsConcreteType<EventStoreRepository>();

                        x.ForRequestedType<ISessionFactory>()
                            .TheDefaultIsConcreteType<EventStoreSessionFactory>();

                    }
                );
            
        }
    }
}

