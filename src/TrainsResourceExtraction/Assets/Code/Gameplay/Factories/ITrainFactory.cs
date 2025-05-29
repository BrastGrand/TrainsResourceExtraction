using UnityEngine;

namespace Code.Gameplay.Factories
{
    public interface ITrainFactory
    {
        GameEntity CreateTrain(int trainId, float movementSpeed, float baseMiningTime, Vector3 position, int startNodeId);
    }
} 