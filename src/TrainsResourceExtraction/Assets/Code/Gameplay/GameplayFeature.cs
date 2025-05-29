using Code.Gameplay.Features.Movement;
using Code.Gameplay.Features.TargetCollection;
using Code.Gameplay.Features.Mining;
using Code.Gameplay.Features.ResourceManagement;
using Code.Gameplay.Features.TrainAI;
using Code.Infrastructure.Systems;

namespace Code.Gameplay
{
    public sealed class GameplayFeature : Feature
    {
        public GameplayFeature(ISystemFactory systems)
        {
            Add(new MovementFeature(systems));
            Add(new CollectTargetsFeature(systems));
            Add(new ResourceManagementFeature(systems));
            Add(new MiningFeature(systems));
            Add(new TrainAIFeature(systems));
        }
    }
} 