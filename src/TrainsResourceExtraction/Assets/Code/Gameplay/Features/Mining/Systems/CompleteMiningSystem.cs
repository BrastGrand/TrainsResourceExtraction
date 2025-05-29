using Entitas;

namespace Code.Gameplay.Features.Mining.Systems
{
    public class CompleteMiningSystem : ReactiveSystem<GameEntity>
    {
        public CompleteMiningSystem(GameContext context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.MiningCompleted);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isMiningCompleted && entity.isMining;
        }

        protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.isResource = true;
                entity.isMining = false;
                entity.isMiningCompleted = false;
                
                entity.isIdleState = true;
                entity.isNeedsNewTarget = true;
            }
        }
    }
} 