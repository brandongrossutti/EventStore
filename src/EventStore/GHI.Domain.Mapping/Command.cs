using System;
using GHI.Bus;

namespace GHI.Domain.Mapping
{
    [Serializable]
    public class Command : Message
    {
        private readonly Guid _aggregateId;

        public Command(Guid aggregateId)
        {
            _aggregateId = aggregateId;
        }

        public Guid AggregateId
        {
            get {
                return _aggregateId;
            }
        }
    }
}
