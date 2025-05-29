using Entitas;
using UnityEngine;
using Code.Gameplay.UI;
using Code.Infrastructure.AssetManagement;
using Code.Gameplay.Services;
using Cysharp.Threading.Tasks;

namespace Code.Gameplay.Features.Visualization.Systems
{
    public class CreateTrainViewReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ISceneHierarchyService _hierarchyService;

        public CreateTrainViewReactiveSystem(GameContext context, IAssetProvider assetProvider, ISceneHierarchyService hierarchyService) : base(context)
        {
            _assetProvider = assetProvider;
            _hierarchyService = hierarchyService;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Train.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasTrain && entity.hasWorldPosition && !entity.hasTransform;
        }

        protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
        {
            var trainContainer = _hierarchyService.GetTrainContainer();
            
            foreach (var entity in entities)
            {
                CreateTrainView(entity, trainContainer).Forget();
            }
        }

        private async UniTask CreateTrainView(GameEntity entity, Transform parent)
        {
            try
            {
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
                
                Debug.Log($"Reactively created TrainView for train {entity.Train} at {position}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create TrainView reactively: {ex.Message}");
            }
        }
    }
} 