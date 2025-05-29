using Entitas;
using UnityEngine;
using Code.Gameplay.UI;
using Code.Infrastructure.AssetManagement;
using Code.Gameplay.Services;
using Cysharp.Threading.Tasks;

namespace Code.Gameplay.Features.Visualization.Systems
{
    public class CreateTrainViewSystem : IInitializeSystem
    {
        private readonly GameContext _context;
        private readonly IAssetProvider _assetProvider;
        private readonly ISceneHierarchyService _hierarchyService;

        public CreateTrainViewSystem(GameContext context, IAssetProvider assetProvider, ISceneHierarchyService hierarchyService)
        {
            _context = context;
            _assetProvider = assetProvider;
            _hierarchyService = hierarchyService;
        }

        public void Initialize()
        {
            CreateTrainViewsForExistingTrains();
        }

        private async void CreateTrainViewsForExistingTrains()
        {
            var trainEntities = _context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.WorldPosition
            ).NoneOf(GameMatcher.Transform));

            var trainContainer = _hierarchyService.GetTrainContainer();

            foreach (var entity in trainEntities.GetEntities())
            {
                await CreateTrainView(entity, trainContainer);
            }
        }

        private async UniTask CreateTrainView(GameEntity entity, Transform parent)
        {
            try
            {
                if (entity.hasTransform)
                {
                    return;
                }
                
                var prefab = await _assetProvider.Load<GameObject>("TrainView");
                if (prefab == null)
                {
                    Debug.LogError("TrainView prefab not found in Addressables!");
                    return;
                }

                var instantiate = Object.Instantiate(prefab, parent);
                var trainView = instantiate.GetComponent<TrainView>();
                
                if (trainView == null)
                {
                    Debug.LogError("TrainView component not found on prefab!");
                    Object.Destroy(instantiate);
                    return;
                }
                
                var position = entity.WorldPosition;
                trainView.transform.position = new Vector3(position.x, position.y, 0);
                
                // Устанавливаем осмысленное имя для объекта
                instantiate.name = $"Train_{entity.Train}";
                
                trainView.Initialize(entity.Train);
                entity.AddTransform(trainView.transform);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create TrainView: {ex.Message}");
            }
        }
    }
} 