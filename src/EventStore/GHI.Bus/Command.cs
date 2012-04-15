using System;

namespace GHI.Bus
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

    [Serializable]
    public class Command<T>:Command  where T: Response
    {
        public Command(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
