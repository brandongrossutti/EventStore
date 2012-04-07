namespace GHI.EventRepository.Impl.SnapShotting
{
    public class NeverSnapShotStrategy : ISnapShotStrategy
    {
        public bool ShouldSnapShot(int lastSnapshotSequence, int currentSequence)
        {
            return false;
        }
    }
}