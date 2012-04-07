namespace GHI.EventRepository.Impl.SnapShotting
{
    public interface ISnapShotStrategy
    {
        bool ShouldSnapShot(int lastSnapshotSequence, int currentSequence);
    }
}