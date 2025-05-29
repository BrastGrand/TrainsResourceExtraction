using Entitas;
using Code.Common.Entity;

namespace Code.Gameplay.Features.ResourceManagement.Systems
{
    public class UpdateTotalResourcesSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;

        public UpdateTotalResourcesSystem(GameContext context) : base(context)
        {
            _context = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.ResourceDelivered);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasResourceDelivered;
        }

        protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
        {
            var totalResourcesGroup = _context.GetGroup(GameMatcher.TotalResources);
            GameEntity totalResourcesEntity;
            
            if (totalResourcesGroup.count == 0)
            {
                totalResourcesEntity = CreateEntity.Empty();
                totalResourcesEntity.AddTotalResources(0);
            }
            else
            {
                totalResourcesEntity = totalResourcesGroup.GetSingleEntity();
            }

            foreach (var entity in entities)
            {
                var currentTotal = totalResourcesEntity.TotalResources;
                var deliveredAmount = entity.ResourceDelivered;
                
                totalResourcesEntity.ReplaceTotalResources(currentTotal + deliveredAmount);
                entity.RemoveResourceDelivered();
            }
        }
    }
}