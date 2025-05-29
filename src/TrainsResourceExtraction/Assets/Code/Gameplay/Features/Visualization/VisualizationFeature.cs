using Code.Gameplay.Features.Visualization.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Visualization
{
    public class VisualizationFeature : Feature
    {
        public VisualizationFeature(ISystemFactory systems)
        {
            Add(systems.Create<CreateNodeViewSystem>());
            Add(systems.Create<CreateTrainViewSystem>());
            Add(systems.Create<CreateTrainViewReactiveSystem>());
            Add(systems.Create<UpdateTrainViewSystem>());
            Add(systems.Create<UpdateNodeViewSystem>());
            Add(systems.Create<GraphVisualizationSystem>());
        }
    }
} 