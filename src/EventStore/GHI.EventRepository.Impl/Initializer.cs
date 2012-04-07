using System;
using EventStore;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository.Impl.UnitOfWork;
using GHI.WireUp;
using StructureMap;

namespace GHI.EventRepository.Impl
{
    public class Initializer
    {
        public static WireUpItem GetWireUp()
        {
            IStoreEvents store = Wireup.Init()
                .LogToOutputWindow()
                .UsingInMemoryPersistence() 
                .EnlistInAmbientTransaction() 
                .InitializeStorageEngine()
                .Build();

            ObjectFactory.Inject<IStoreEvents>(store);

            return new WireUpItem(
                x =>
                    {
                        x.For<IRepository<Guid>>()
                            .Singleton()
                            .Use<EventStoreRepository>();

                        x.For<IUnitOfWorkFactory>()
                            .Use<EventStoreUnitOfWorkFactory>();
                    }
                );
            
        }
    }
}

