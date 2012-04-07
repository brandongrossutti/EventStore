using System;
using System.Collections.Generic;

namespace GHI.EventRepository.Impl.SnapShotting
{
    public class SnapShotTracker
    {
        private readonly ISnapShotStrategy _snapShotStrategy;
        private readonly Dictionary<Guid, int> _snapShotSequences;
        private readonly Dictionary<Guid, int> _commitSequences;
        public SnapShotTracker(ISnapShotStrategy snapShotStrategy)
        {
            _snapShotStrategy = snapShotStrategy;
            _snapShotSequences=new Dictionary<Guid, int>();
            _commitSequences = new Dictionary<Guid, int>();
        }

        public int GetLastSequence(Guid id)
        {
            int sequence = 0;
            if (!_snapShotSequences.ContainsKey(id))
            {
                _snapShotSequences.Add(id, sequence);
            }
            else
            {
                sequence = _snapShotSequences[id];
            }
            return sequence;
        }

        public void SetLastSequence(Guid id, int sequence)
        {
            if (!_snapShotSequences.ContainsKey(id))
            {
                _snapShotSequences.Add(id, sequence);
            }
            else
            {
                _snapShotSequences[id] = sequence;
            }
        }

        public bool ShouldSnapShot(Guid id, int commitSequence)
        {
            if (!_commitSequences.ContainsKey(id))
            {
                _commitSequences.Add(id, commitSequence);
            }
            else
            {
                _commitSequences[id] = commitSequence;
            }
            return _snapShotStrategy.ShouldSnapShot(GetLastSequence(id), commitSequence);
        }
    }
}