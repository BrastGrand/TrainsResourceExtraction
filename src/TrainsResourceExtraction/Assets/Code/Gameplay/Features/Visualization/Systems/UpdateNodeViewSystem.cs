using UnityEngine;
using Entitas;
using Code.Gameplay.Features.Graph;

namespace Code.Gameplay.Features.Visualization.Systems
{
    public class UpdateNodeViewSystem : IExecuteSystem
    {
        private readonly IGraphService _graphService;
        private readonly NodeViewRegistry _nodeViewRegistry;

        public UpdateNodeViewSystem(IGraphService graphService, NodeViewRegistry nodeViewRegistry)
        {
            _graphService = graphService;
            _nodeViewRegistry = nodeViewRegistry;
        }

        public void Execute()
        {
            // В простой реализации - проверяем каждые 30 кадров
            // В production версии лучше использовать событийную модель
            if (Time.frameCount % 30 == 0)
            {
                UpdateAllNodeViews();
            }
        }

        /// <summary>
        /// Принудительно обновляет все NodeView
        /// </summary>
        public void ForceUpdate()
        {
            UpdateAllNodeViews();
        }

        /// <summary>
        /// Обновляет все NodeView - публичный метод для внешних вызовов
        /// </summary>
        public void UpdateAllNodeViews()
        {
            var allNodes = _graphService.GetAllNodes();
            
            foreach (var node in allNodes)
            {
                UpdateNodeView(node);
            }
        }

        private void UpdateNodeView(GraphNode node)
        {
            var nodeView = _nodeViewRegistry.GetNodeView(node.Id);

            if (nodeView == null) return;
            float currentMultiplier = node.GetMultiplier();
            nodeView.UpdateMultiplier(currentMultiplier);
        }
    }
} 