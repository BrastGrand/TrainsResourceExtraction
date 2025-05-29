using System.Collections.Generic;
using Code.Gameplay.UI;

namespace Code.Gameplay.Features.Visualization
{
    public class NodeViewRegistry
    {
        private readonly Dictionary<int, NodeView> _nodeViews = new();

        /// <summary>
        /// Регистрирует NodeView для конкретного узла
        /// </summary>
        public void RegisterNodeView(int nodeId, NodeView nodeView)
        {
            _nodeViews[nodeId] = nodeView;
        }

        /// <summary>
        /// Получает NodeView для конкретного узла
        /// </summary>
        public NodeView GetNodeView(int nodeId)
        {
            return _nodeViews.TryGetValue(nodeId, out var nodeView) ? nodeView : null;
        }

        /// <summary>
        /// Получает все зарегистрированные NodeView
        /// </summary>
        public IEnumerable<NodeView> GetAllNodeViews()
        {
            return _nodeViews.Values;
        }

        /// <summary>
        /// Удаляет NodeView из реестра
        /// </summary>
        public void UnregisterNodeView(int nodeId)
        {
            _nodeViews.Remove(nodeId);
        }

        /// <summary>
        /// Очищает весь реестр
        /// </summary>
        public void Clear()
        {
            _nodeViews.Clear();
        }
    }
} 