using Code.Gameplay.Common.Time;
using Entitas;

namespace Code.Gameplay.Features.Movement.Systems
{
    public class DirectionalDeltaMoveSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;
        private readonly ITimeService _time;

        public DirectionalDeltaMoveSystem(GameContext game, ITimeService time)
        {
            _time = time;
            _movers = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Direction,
                    GameMatcher.WorldPosition,
                    GameMatcher.Speed,
                    GameMatcher.MovementAvailable,
                    GameMatcher.Moving));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _movers)
            {
                var newPosition = entity.WorldPosition + entity.Direction * entity.Speed * _time.DeltaTime;
                entity.ReplaceWorldPosition(newPosition);
            }
        }
    }
}