using System;
using System.Collections.Generic;
using EventStore;

namespace GHI.EventRepository.Impl
{
    class EventStream : IEventStream
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Add(EventMessage uncommittedEvent)
        {
            throw new NotImplementedException();
        }

        public void CommitChanges(Guid commitId)
        {
            throw new NotImplementedException();
        }

        public void ClearChanges()
        {
            throw new NotImplementedException();
        }

        public Guid StreamId
        {
            get { throw new NotImplementedException(); }
        }

        public int StreamRevision
        {
            get { throw new NotImplementedException(); }
        }

        public int CommitSequence
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<EventMessage> CommittedEvents
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, object> CommittedHeaders
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<EventMessage> UncommittedEvents
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, object> UncommittedHeaders
        {
            get { throw new NotImplementedException(); }
        }
    }
}
