using UnityEngine;
using Entitas;
using Code.Gameplay.Features.Graph;
using Code.Gameplay.UI;
using Code.Infrastructure.AssetManagement;
using Code.Gameplay.Services;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.Gameplay.Features.Visualization.Systems
{
    public class CreateNodeViewSystem : IInitializeSystem
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IGraphService _graphService;
        private readonly NodeViewRegistry _nodeViewRegistry;
        private readonly ISceneHierarchyService _hierarchyService;

        public CreateNodeViewSystem(IAssetProvider assetProvider, IGraphService graphService, NodeViewRegistry nodeViewRegistry, ISceneHierarchyService hierarchyService)
        {
            _assetProvider = assetProvider;
            _graphService = graphService;
            _nodeViewRegistry = nodeViewRegistry;
            _hierarchyService = hierarchyService;
        }

        public void Initialize()
        {
            CreateNodeViews();
        }

        private async void CreateNodeViews()
        {
            var allNodes = _graphService.GetAllNodes();
            var nodesContainer = _hierarchyService.GetNodesContainer();

            foreach (var node in allNodes)
            {
                await CreateNodeView(node, nodesContainer);
            }
        }

        private async UniTask CreateNodeView(GraphNode node, Transform parent)
        {
            string prefabAddress = ConvertNodeTypeToText(node.GetNodeType());
            
            var prefab = await _assetProvider.Load<GameObject>(prefabAddress);
            var instantiate = Object.Instantiate(prefab, parent);
            instantiate.transform.position = node.Position;
            
            // Устанавливаем имя для объекта
            instantiate.name = $"{node.GetNodeType()}_{node.Id}";

            if (node.GetNodeType() == NodeType.Empty)
            {
                var emptyNode = instantiate.GetComponent<EmptyNodeView>();
                emptyNode.Initialize(node.Id);
            }
            else
            {
                var nodeView = instantiate.GetComponent<NodeView>();
                bool isMine = node.GetNodeType() == NodeType.Mine;
                float multiplier = node.GetMultiplier();

                nodeView.Initialize(node.Id, multiplier, isMine);
                
                _nodeViewRegistry.RegisterNodeView(node.Id, nodeView);
            }
        }

        private string ConvertNodeTypeToText(NodeType type)
        {
            switch (type)
            {
                case NodeType.Base: return "BaseNode";
                case NodeType.Mine: return "MineNode";
                case NodeType.Empty: return "EmptyNode";
                default: return "EmptyNode";
            }
        }
    }
} 