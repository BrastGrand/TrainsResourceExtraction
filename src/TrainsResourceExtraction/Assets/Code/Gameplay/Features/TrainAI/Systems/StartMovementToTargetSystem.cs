using Entitas;
using Code.Gameplay.Features.Graph;
using UnityEngine;
using System.Collections.Generic;

namespace Code.Gameplay.Features.TrainAI.Systems
{
    public class StartMovementToTargetSystem : IExecuteSystem
    {
        private readonly IGraphService _graphService;
        private readonly IGroup<GameEntity> _trainsReadyToMove;
        private readonly List<GameEntity> _buffer = new(16);

        public StartMovementToTargetSystem(GameContext context, IGraphService graphService)
        {
            _graphService = graphService;
            _trainsReadyToMove = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.TargetNode,
                GameMatcher.CurrentNode,
                GameMatcher.IdleState
            ).NoneOf(
                GameMatcher.Moving,
                GameMatcher.NextNodeInPath
            ));
        }

        public void Execute()
        {
            foreach (var entity in _trainsReadyToMove.GetEntities(_buffer))
            {
                var currentNodeId = entity.CurrentNode;
                var targetNodeId = entity.TargetNode;

                var path = _graphService.FindShortestPath(currentNodeId, targetNodeId);
                if (path.Count > 1)
                {
                    var nextNode = path[1];
                    var edge = _graphService.GetEdge(currentNodeId, nextNode.Id);
                    
                    if (edge != null)
                    {
                        var targetDestination = new Vector2(nextNode.Position.x, nextNode.Position.y);
                        entity.AddTargetDestination(targetDestination);
                        
                        var direction = (targetDestination - entity.WorldPosition).normalized;
                        entity.ReplaceDirection(direction);
                        
                        entity.isMoving = true;
                        entity.isIdleState = false;
                        
                        // Сохраняем следующий узел в пути
                        entity.AddNextNodeInPath(nextNode.Id);
                    }
                    else
                    {
                        Debug.LogError($"No edge found between nodes {currentNodeId} and {nextNode.Id}");
                    }
                }
                else if (path.Count == 1)
                {
                    entity.isIdleState = true;
                    entity.isNeedsNewTarget = true;
                    entity.RemoveTargetNode();
                }
                else
                {
                    Debug.LogWarning($"No path found from node {currentNodeId} to node {targetNodeId}");
                    entity.isIdleState = true;
                    entity.isNeedsNewTarget = true;
                    entity.RemoveTargetNode();
                }
            }
        }
    }
} 