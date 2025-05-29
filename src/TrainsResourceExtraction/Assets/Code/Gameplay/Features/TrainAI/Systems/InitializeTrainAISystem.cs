using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.TrainAI.Systems
{
    public class InitializeTrainAISystem : IInitializeSystem
    {
        private readonly GameContext _context;

        public InitializeTrainAISystem(GameContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            // Находим всех поездов без активного ИИ
            var trainEntities = _context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.IdleState
            ).NoneOf(GameMatcher.NeedsNewTarget));

            Debug.Log($"Initializing AI for {trainEntities.count} trains");

            foreach (var entity in trainEntities.GetEntities())
            {
                // Активируем ИИ для поездов
                entity.isNeedsNewTarget = true;
                Debug.Log($"Initialized AI for train {entity.Train} at node {entity.CurrentNode}");
            }
        }
    }
} 