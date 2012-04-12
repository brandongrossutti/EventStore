using System;
using System.Collections.Generic;
using GHI.Commons.UnitOfWork;
using GHI.Domain;

namespace GHI.EventRepository.Impl.UnitOfWork
{
    public class EventStoreUnitOfWork : IUnitOfWork
    {
        private readonly EventStoreRepository _eventStorage;
        private readonly HashSet<AggregateRoot> _aggregateRootsAffected = new HashSet<AggregateRoot>();

        [ThreadStatic]
        private static EventStoreUnitOfWork _current;

        internal EventStoreUnitOfWork(IRepository<Guid> eventStorage)
        {
            _eventStorage = eventStorage as EventStoreRepository;
            if (_current != null)
                throw new InvalidOperationException("Cannot nest unit of work");

            _current = this;
        }

        private static EventStoreUnitOfWork Current
        {
            get { return _current; }
        }

        public void Dispose()
        {
            _current = null;
        }

        public static void RegisterAggregateRoot<T>(AggregateRoot aggregateRoot)
        {
            var unitOfWork = Current;
            if (!unitOfWork._aggregateRootsAffected.Contains(aggregateRoot))
            {
                unitOfWork._aggregateRootsAffected.Add(aggregateRoot);
            }
        }

        public void Commit()
        {
            foreach (AggregateRoot aggregateRoot in _aggregateRootsAffected)
            {
                _eventStorage.Commit(aggregateRoot);
            }
            _aggregateRootsAffected.Clear();
        }

        public void RollBack()
        {
            foreach (AggregateRoot aggregateRoot in _aggregateRootsAffected)
            {
                aggregateRoot.ClearUncommitedEvents();
                aggregateRoot.ReloadAggregateRoot();
            }
        }
    }
}