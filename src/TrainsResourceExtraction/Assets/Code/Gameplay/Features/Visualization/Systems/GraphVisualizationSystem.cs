using UnityEngine;
using Entitas;
using Code.Gameplay.Features.Graph;
using Code.Gameplay.Services;

namespace Code.Gameplay.Features.Visualization.Systems
{
    /// <summary>
    /// Система для визуализации графа и путей поездов через Gizmos
    /// Рисует ребра графа и текущие маршруты поездов в SceneView
    /// </summary>
    public class GraphVisualizationSystem : IInitializeSystem
    {
        private readonly IGraphService _graphService;
        private readonly GameContext _gameContext;
        private readonly ISceneHierarchyService _hierarchyService;
        private GraphGizmosDrawer _gizmosDrawer;

        public GraphVisualizationSystem(IGraphService graphService, GameContext gameContext, ISceneHierarchyService hierarchyService)
        {
            _graphService = graphService;
            _gameContext = gameContext;
            _hierarchyService = hierarchyService;
        }

        public void Initialize()
        {
            CreateGizmosDrawer();
        }

        private void CreateGizmosDrawer()
        {
            var systemContainer = _hierarchyService.GetSystemObjectsContainer();
            var gizmosObject = new GameObject("GraphVisualization");
            gizmosObject.transform.SetParent(systemContainer);
            
            _gizmosDrawer = gizmosObject.AddComponent<GraphGizmosDrawer>();
            _gizmosDrawer.Initialize(_graphService, _gameContext);
        }
    }
}