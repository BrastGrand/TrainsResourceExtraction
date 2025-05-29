using Entitas;

namespace Code.Gameplay.Features.Movement.Systems
{
    public class StopMovementOnReachedSystem : ReactiveSystem<GameEntity>
    {
        public StopMovementOnReachedSystem(GameContext context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(
                GameMatcher.Reached,
                GameMatcher.Moving
            ));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isReached && entity.isMoving;
        }

        protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.isMoving = false;
                
                if (entity.hasDirection)
                {
                    entity.RemoveDirection();
                }
            }
        }
    }
} 