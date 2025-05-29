using Code.Gameplay.Features.Mining.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Mining
{
    public class MiningFeature : Feature
    {
        public MiningFeature(ISystemFactory systems)
        {
            Add(systems.Create<ProcessMiningSystem>());
            Add(systems.Create<CompleteMiningSystem>());
        }
    }
} 