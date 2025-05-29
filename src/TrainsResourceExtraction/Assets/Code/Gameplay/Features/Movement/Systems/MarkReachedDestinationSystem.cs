using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Movement.Systems
{
    public class MarkReachedDestinationSystem : IExecuteSystem
    {
        private const float _REACH_DISTANCE = 2.0f;

        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(16);

        public MarkReachedDestinationSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TargetDestination,
                    GameMatcher.WorldPosition)
                .NoneOf(GameMatcher.Reached));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                var distance = Vector3.Distance(entity.WorldPosition, entity.TargetDestination);
                if (distance <= _REACH_DISTANCE)
                {
                    entity.isReached = true;
                }
            }
        }
    }
}