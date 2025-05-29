using Entitas;
using Code.Gameplay.Features.Graph;
using System.Collections.Generic;

namespace Code.Gameplay.Features.TrainAI.Systems
{
    public class TrainStateManagerSystem : IExecuteSystem
    {
        private readonly IGraphService _graphService;
        private readonly IGroup<GameEntity> _trains;
        private readonly List<GameEntity> _buffer = new(16);

        public TrainStateManagerSystem(GameContext context, IGraphService graphService)
        {
            _graphService = graphService;
            _trains = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.CurrentNode
            ));
        }

        public void Execute()
        {
            var entities = _trains.GetEntities(_buffer);

            foreach (var entity in entities)
            {
                ProcessTrainState(entity);
            }
        }

        private void ProcessTrainState(GameEntity entity)
        {
            var currentNodeId = entity.CurrentNode;
            var currentNode = _graphService.GetNode(currentNodeId);
            
            if (entity.isReached)
            {
                HandleNodeArrival(entity);
                return;
            }
            
            if (entity.isMoving)
            {
                return;
            }

            if (entity.isMining)
            {
                return;
            }

            if (currentNode?.GetNodeType() == NodeType.Mine && !entity.isResource && !entity.isMining)
            {
                StartMining(entity, currentNodeId);
                return;
            }

            if (currentNode?.GetNodeType() == NodeType.Base && entity.isResource)
            {
                DeliverResource(entity, currentNodeId, currentNode as BaseNode);
                return;
            }

            if (currentNode?.GetNodeType() == NodeType.Empty && entity.hasTargetNode)
            {
                PrepareMovement(entity);
                return;
            }

            if (!entity.hasTargetNode && !entity.isNeedsNewTarget)
            {
                MarkAsNeedingNewTarget(entity, currentNodeId);
                return;
            }

            if (entity.hasTargetNode && !entity.isMoving && !entity.hasNextNodeInPath)
            {
                PrepareMovement(entity);
            }
        }

        private void HandleNodeArrival(GameEntity entity)
        {
            // Если есть NextNodeInPath - это промежуточный узел
            if (entity.hasNextNodeInPath)
            {
                var nextNodeId = entity.NextNodeInPath;
                var finalTargetId = entity.hasTargetNode ? entity.TargetNode : -1;

                // Обновляем текущий узел
                entity.ReplaceCurrentNode(nextNodeId);
                entity.RemoveNextNodeInPath();
                entity.isMoving = false;
                entity.isReached = false;
                entity.RemoveTargetDestination();

                if (nextNodeId == finalTargetId)
                {
                    entity.RemoveTargetNode();
                }
                else
                {
                    entity.isIdleState = true;
                }
            }
            else
            {
                entity.isMoving = false;
                entity.isReached = false;
                entity.RemoveTargetDestination();
                
                if (entity.hasTargetNode)
                {
                    entity.RemoveTargetNode();
                }
            }
        }

        private void StartMining(GameEntity entity, int nodeId)
        {
            var mineNode = _graphService.GetNode(nodeId) as MineNode;
            if (mineNode == null) return;

            var baseMiningTime = entity.BaseMiningTime;
            var miningTime = mineNode.CalculateMiningTime(baseMiningTime);

            entity.isMining = true;
            entity.isIdleState = false;
            entity.AddMiningTime(miningTime);
        }

        private void DeliverResource(GameEntity entity, int nodeId, BaseNode baseNode)
        {
            if (baseNode == null)
            {
                return;
            }

            var deliveredAmount = baseNode.ProcessResourceDelivery(1);
            
            entity.AddResourceDelivered(deliveredAmount);
            entity.isResource = false;

            // Сразу помечаем как нуждающегося в новой цели
            MarkAsNeedingNewTarget(entity, nodeId);
        }

        private void MarkAsNeedingNewTarget(GameEntity entity, int nodeId)
        {
            entity.isNeedsNewTarget = true;
            entity.isIdleState = true;
        }

        private void PrepareMovement(GameEntity entity)
        {
            entity.isIdleState = true;
        }
    }
} 