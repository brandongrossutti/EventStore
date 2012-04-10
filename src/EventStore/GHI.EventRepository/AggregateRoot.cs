using System;
using System.Collections.Generic;
using System.Reflection;

namespace GHI.EventRepository
{
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> _uncommitedEvents;
        public abstract Guid Id { get; }

        public AggregateRoot()
        {
            _uncommitedEvents = new List<IEvent>();
        }

        public IEnumerable<IEvent> UncommittedEvents
        {
            get { return _uncommitedEvents; }
        }

        public bool HasUncommittedEvents    
        {
            get { return _uncommitedEvents.Count > 0; }
        }

        protected void OnEvent(IEvent @event)
        {
            OnEvent(@event, true);
        }

        private void OnEvent(IEvent @event, bool isNew)
        {
            string eventName = "On" + @event.GetType().Name.Replace("Event", "");
            MethodInfo method = GetType().GetMethod(eventName);
            method.Invoke(this, new object[] { @event });
            if (isNew)_uncommitedEvents.Add(@event);
        }

        public void ClearUncommitedEvents()
        {
            _uncommitedEvents.Clear();
        }

        public void LoadFromRepository(IEnumerable<IEvent> events)
        {
            foreach (IEvent @event in events)
            {
                OnEvent(@event, false);
            }
        }

        /// <summary>
        /// TODO
        /// we have affected state of aggregate root but are not commiting those events to store if rollbackis called in UoW
        /// we need to relaod our aggregate to the state that it was prior to the last events commited
        /// </summary>
        public void ReloadAggregateRoot(){}
    }
}