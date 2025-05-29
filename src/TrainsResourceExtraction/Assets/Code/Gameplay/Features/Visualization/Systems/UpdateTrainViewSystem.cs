using Entitas;
using Code.Gameplay.UI;
using UnityEngine;

namespace Code.Gameplay.Features.Visualization.Systems
{
    public class UpdateTrainViewSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _trainEntities;

        public UpdateTrainViewSystem(GameContext context)
        {
            _trainEntities = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.Transform,
                GameMatcher.WorldPosition
            ));
        }

        public void Execute()
        {
            foreach (var entity in _trainEntities.GetEntities())
            {
                UpdatePosition(entity);
                UpdateTrainView(entity);
            }
        }

        private void UpdatePosition(GameEntity entity)
        {
            var position = entity.WorldPosition;
            entity.Transform.position = new Vector3(position.x, position.y, 0);
        }

        private void UpdateTrainView(GameEntity entity)
        {
            var trainView = entity.Transform.GetComponent<TrainView>();
            if (trainView == null) return;

            // Обновляем скорость
            if (entity.hasSpeed)
            {
                trainView.UpdateSpeed(entity.Speed);
            }

            // Обновляем статус
            string status = GetTrainStatus(entity);
            trainView.UpdateStatus(status);
        }

        private string GetTrainStatus(GameEntity entity)
        {
            if (entity.isMining) return "Mining";
            if (entity.isMoving) return "Moving";
            if (entity.hasNextNodeInPath) return "InTransit";
            if (entity.isResource) return "HasResource";
            if (entity.isIdleState && entity.isNeedsNewTarget) return "SelectingTarget";
            return entity.isIdleState ? "Idle" : "Unknown";
        }
    }
} 