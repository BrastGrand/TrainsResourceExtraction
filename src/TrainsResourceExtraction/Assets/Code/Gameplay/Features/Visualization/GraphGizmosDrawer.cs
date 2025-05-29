using System.Collections.Generic;
using Code.Gameplay.Features.Graph;
using UnityEngine;

namespace Code.Gameplay.Features.Visualization
{
    public class GraphGizmosDrawer : MonoBehaviour
    {
        private IGraphService _graphService;
        private GameContext _gameContext;
        private List<GameEntity> _trainBuffer = new(16);

        public void Initialize(IGraphService graphService, GameContext gameContext)
        {
            _graphService = graphService;
            _gameContext = gameContext;
        }

        private void OnDrawGizmos()
        {
            if (_graphService == null) return;

            DrawGraphEdges();
            DrawTrainPaths();
        }

        /// <summary>
        /// Рисует все ребра графа как серые линии
        /// </summary>
        private void DrawGraphEdges()
        {
            var allNodes = _graphService.GetAllNodes();

            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f); // Полупрозрачный серый

            foreach (var node in allNodes)
            {
                var edges = _graphService.GetEdgesFromNode(node.Id);
                foreach (var edge in edges)
                {
                    var toNode = _graphService.GetNode(edge.ToNode.Id);
                    if (toNode != null)
                    {
                        Gizmos.DrawLine(node.Position, toNode.Position);

                        // Рисуем стрелку для направления
                        DrawArrow(node.Position, toNode.Position);
                    }
                }
            }
        }

        /// <summary>
        /// Рисует текущие пути поездов яркими цветами
        /// </summary>
        private void DrawTrainPaths()
        {
            if (_gameContext == null) return;

            var trains = _gameContext.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.CurrentNode
            ));

            _trainBuffer.Clear();
            trains.GetEntities(_trainBuffer);

            foreach (var train in _trainBuffer)
            {
                DrawTrainPath(train);
            }
        }

        /// <summary>
        /// Рисует путь конкретного поезда
        /// </summary>
        private void DrawTrainPath(GameEntity train)
        {
            // Устанавливаем цвет в зависимости от ID поезда
            Gizmos.color = GetTrainColor(train.Train);

            var currentNodeId = train.CurrentNode;
            var currentNode = _graphService.GetNode(currentNodeId);

            if (currentNode == null) return;

            // Рисуем позицию поезда
            Gizmos.DrawWireSphere(currentNode.Position, 0.8f);

            // Если поезд имеет цель - рисуем путь к ней
            if (train.hasTargetNode)
            {
                var targetNodeId = train.TargetNode;
                var path = _graphService.FindShortestPath(currentNodeId, targetNodeId);

                DrawPath(path, Gizmos.color);

                // Рисуем цель особым образом
                var targetNode = _graphService.GetNode(targetNodeId);
                if (targetNode != null)
                {
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.8f);
                    Gizmos.DrawSphere(targetNode.Position, 0.5f);
                }
            }

            // Если поезд движется - рисуем следующий узел в пути
            if (train.hasNextNodeInPath)
            {
                var nextNodeId = train.NextNodeInPath;
                var nextNode = _graphService.GetNode(nextNodeId);

                if (nextNode != null)
                {
                    // Рисуем толстую линию до следующего узла
                    DrawThickLine(currentNode.Position, nextNode.Position, 0.2f);

                    // Анимированная точка движения
                    if (train.hasTargetDestination)
                    {
                        var targetPos = train.TargetDestination;
                        var animatedPos = Vector3.Lerp(currentNode.Position, new Vector3(targetPos.x, targetPos.y, 0),
                            Mathf.PingPong(Time.time * 2f, 1f));

                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(animatedPos, 0.3f);
                    }
                }
            }
        }

        /// <summary>
        /// Рисует путь как последовательность линий
        /// </summary>
        private void DrawPath(List<GraphNode> path, Color color)
        {
            if (path.Count < 2) return;

            var originalColor = Gizmos.color;
            Gizmos.color = color;

            for (int i = 0; i < path.Count - 1; i++)
            {
                var from = path[i].Position;
                var to = path[i + 1].Position;

                // Делаем линию толще для путей поездов
                DrawThickLine(from, to, 0.1f);
            }

            Gizmos.color = originalColor;
        }

        /// <summary>
        /// Рисует толстую линию через несколько тонких линий
        /// </summary>
        private void DrawThickLine(Vector3 from, Vector3 to, float thickness)
        {
            var direction = (to - from).normalized;
            var perpendicular = Vector3.Cross(direction, Vector3.forward) * thickness;

            // Рисуем несколько параллельных линий для создания эффекта толщины
            for (int i = -2; i <= 2; i++)
            {
                var offset = perpendicular * (i * 0.2f);
                Gizmos.DrawLine(from + offset, to + offset);
            }
        }

        /// <summary>
        /// Рисует стрелку для показа направления ребра
        /// </summary>
        private void DrawArrow(Vector3 from, Vector3 to)
        {
            var direction = (to - from).normalized;
            var arrowHead = to - direction * 1f; // Стрелка на расстоянии 1 от конца
            var perpendicular = Vector3.Cross(direction, Vector3.forward) * 0.5f;

            Gizmos.DrawLine(arrowHead + perpendicular, to);
            Gizmos.DrawLine(arrowHead - perpendicular, to);
        }

        /// <summary>
        /// Возвращает цвет для конкретного поезда
        /// </summary>
        private Color GetTrainColor(int trainId)
        {
            return trainId switch
            {
                1 => Color.red,      // Поезд 1 - красный
                2 => Color.green,    // Поезд 2 - зеленый
                3 => Color.blue,     // Поезд 3 - синий
                _ => Color.white
            };
        }
    }
}