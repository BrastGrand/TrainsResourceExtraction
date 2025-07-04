using Code.Gameplay.Features.TargetCollection.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.TargetCollection
{
    public class CollectTargetsFeature : Feature
    {
        public CollectTargetsFeature(ISystemFactory systems)
        {
            Add(systems.Create<CollectTargetsIntervalSystem>());
            Add(systems.Create<MarkReachedOnBufferFillSystem>());
            Add(systems.Create<CleanupTargetBuffersSystem>());
        }
    }
}