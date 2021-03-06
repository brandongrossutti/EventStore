﻿using System;
using EventStore;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository.Impl.SnapShotting;
using GHI.EventRepository.Impl.UnitOfWork;
using GHI.WireUp;
using StructureMap;

namespace GHI.EventRepository.Impl
{
    public class Initializer : IInitializer
    {
        public WireUpItem GetWireUp(InitializerWireUp wireup)
        {
            IStoreEvents store = Wireup.Init()
                .LogToOutputWindow()
                //.UsingSqlPersistence()
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

                        x.For<ISnapShotStrategy>()
                            .Singleton()
                            .Use<SnapShotBasedOnMessageCountStrategy>()
                            .Ctor<int>().Is(3);
                    }
                );
        }
    }
}

