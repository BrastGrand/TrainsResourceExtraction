using Code.Common.Entity;
using Code.Infrastructure.Identifiers;
using UnityEngine;

namespace Code.Gameplay.Factories
{
    public class TrainFactory : ITrainFactory
    {
        private readonly IIdentifierService _identifierService;

        public TrainFactory(IIdentifierService identifierService)
        {
            _identifierService = identifierService;
        }

        public GameEntity CreateTrain(int trainId, float movementSpeed, float baseMiningTime, Vector3 position, int startNodeId)
        {
            var entity = CreateEntity.Empty();
            
            entity.AddId(_identifierService.Next());
            entity.AddTrain(trainId);
            entity.AddSpeed(movementSpeed);
            entity.AddBaseMiningTime(baseMiningTime);
            entity.AddWorldPosition(new Vector2(position.x, position.y));
            entity.AddCurrentNode(startNodeId);
            entity.isMovementAvailable = true;

            return entity;
        }
    }
} 