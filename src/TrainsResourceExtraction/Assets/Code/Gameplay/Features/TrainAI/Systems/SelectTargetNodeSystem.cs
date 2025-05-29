using Entitas;
using Code.Gameplay.Features.Graph;
using Code.Gameplay.Common.Random;
using UnityEngine;
using System.Collections.Generic;

namespace Code.Gameplay.Features.TrainAI.Systems
{
    public class SelectTargetNodeSystem : IExecuteSystem
    {
        private readonly IGraphService _graphService;
        private readonly IRandomService _randomService;
        private readonly IGroup<GameEntity> _trainsNeedingTarget;
        private readonly List<GameEntity> _buffer = new(16);

        public SelectTargetNodeSystem(GameContext context, IGraphService graphService, IRandomService randomService)
        {
            _graphService = graphService;
            _randomService = randomService;
            _trainsNeedingTarget = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.NeedsNewTarget,
                GameMatcher.CurrentNode
            ).NoneOf(
                GameMatcher.TargetNode,
                GameMatcher.Moving
            ));
        }

        public void Execute()
        {
            foreach (var entity in _trainsNeedingTarget.GetEntities(_buffer))
            {
                var currentNodeId = entity.CurrentNode;
                var hasResource = entity.isResource;

                var targetNodes = hasResource 
                    ? _graphService.GetNodesByType(NodeType.Base)
                    : _graphService.GetNodesByType(NodeType.Mine);

                if (targetNodes.Count > 0)
                {
                    var targetNode = FindBestTarget(entity, currentNodeId, targetNodes, hasResource);
                    if (targetNode != null)
                    {
                        entity.AddTargetNode(targetNode.Id);
                        entity.isNeedsNewTarget = false;
                    }
                }
                else
                {
                    Debug.LogWarning($"No target nodes of type {(hasResource ? "Base" : "Mine")} available");
                }
            }
        }

        private GraphNode FindBestTarget(GameEntity trainEntity, int fromNodeId, List<GraphNode> targetNodes, bool hasResource)
        {
            GraphNode bestTarget = null;
            var bestEfficiency = float.MinValue;

            foreach (var targetNode in targetNodes)
            {
                var path = _graphService.FindShortestPath(fromNodeId, targetNode.Id);
                if (path.Count > 1)
                {
                    float efficiency = CalculateNodeEfficiency(trainEntity, targetNode, path, hasResource);

                    if (efficiency > bestEfficiency)
                    {
                        bestEfficiency = efficiency;
                        bestTarget = targetNode;
                    }
                }
            }

            return bestTarget ?? targetNodes[_randomService.Range(0, targetNodes.Count)];
        }

        /// <summary>
        /// Рассчитывает эффективность узла для данного поезда
        /// Учитывает множители, время добычи, расстояние и скорость поезда
        /// </summary>
        private float CalculateNodeEfficiency(GameEntity trainEntity, GraphNode targetNode, List<GraphNode> path, bool hasResource)
        {
            // Базовые параметры поезда
            float trainSpeed = trainEntity.hasSpeed ? trainEntity.Speed : 10f;
            float baseMiningTime = trainEntity.hasBaseMiningTime ? trainEntity.BaseMiningTime : 5f;
            
            // Расчитываем время пути
            float pathLength = CalculatePathLength(path);
            float travelTime = pathLength / trainSpeed;
            
            if (hasResource)
            {
                // Для баз: эффективность = ресурсный множитель / время пути
                // Чем больше множитель и меньше время пути - тем лучше
                var baseNode = targetNode as BaseNode;
                float resourceMultiplier = baseNode?.ResourceMultiplier ?? 1f;
                
                return resourceMultiplier / (travelTime + 1f); // +1 чтобы избежать деления на 0
            }

            // Для шахт: эффективность = 1 / (время добычи * время пути)
            // Чем меньше общее время (добыча + путь) - тем лучше
            var mineNode = targetNode as MineNode;
            float miningTime = mineNode?.CalculateMiningTime(baseMiningTime) ?? baseMiningTime;
            float totalTime = miningTime + travelTime;
                
            return 1f / (totalTime + 1f); // +1 чтобы избежать деления на 0
        }

        private float CalculatePathLength(List<GraphNode> path)
        {
            float totalLength = 0f;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var edge = _graphService.GetEdge(path[i].Id, path[i + 1].Id);
                if (edge != null)
                    totalLength += edge.Length;
            }
            return totalLength;
        }
    }
}