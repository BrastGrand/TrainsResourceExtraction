using Entitas;
using Code.Gameplay.Services;

namespace Code.Gameplay.Features.ResourceManagement.Systems
{
    public class UpdateResourcesUISystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIService _uiService;

        public UpdateResourcesUISystem(GameContext context, IUIService uiService) : base(context)
        {
            _uiService = uiService;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AnyOf(
                GameMatcher.TotalResources,
                GameMatcher.ResourceDelivered
            ));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasTotalResources || entity.hasResourceDelivered;
        }

        protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
        {
            var resourcesDisplay = _uiService.GetResourcesDisplay();
            if (resourcesDisplay == null) return;

            foreach (var entity in entities)
            {
                if (entity.hasTotalResources)
                {
                    resourcesDisplay.UpdateTotalResources(entity.TotalResources);
                }

                if (entity.hasResourceDelivered)
                {
                    resourcesDisplay.UpdateDeliveredResources(entity.ResourceDelivered);
                }
            }
        }
    }
} 