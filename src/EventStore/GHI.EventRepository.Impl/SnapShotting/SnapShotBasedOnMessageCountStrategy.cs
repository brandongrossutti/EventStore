namespace GHI.EventRepository.Impl.SnapShotting
{
    public class SnapShotBasedOnMessageCountStrategy : ISnapShotStrategy
    {
        private readonly int _maxDifference;

        public SnapShotBasedOnMessageCountStrategy(int maxDifference)
        {
            _maxDifference = maxDifference;
        }

        public bool ShouldSnapShot(int lastSnapshotSequence, int currentSequence)
        {
            if ((currentSequence - lastSnapshotSequence) >= _maxDifference)
                return true;
            return false;
        }
    }
}