using Code.Gameplay.Features.TrainAI.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.TrainAI
{
    public class TrainAIFeature : Feature
    {
        public TrainAIFeature(ISystemFactory systems)
        {
            Add(systems.Create<InitializeTrainAISystem>());
            Add(systems.Create<TrainStateManagerSystem>());
            Add(systems.Create<SelectTargetNodeSystem>());
            Add(systems.Create<StartMovementToTargetSystem>());
        }
    }
} 