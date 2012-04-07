using System;
using System.Collections.Generic;

namespace GHI.EventRepository.Impl
{
    public class SnapShotTracker
    {
        private readonly Dictionary<Guid, int> _snapShotSequences;
        public SnapShotTracker()
        {
            _snapShotSequences=new Dictionary<Guid, int>();
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
    }
}