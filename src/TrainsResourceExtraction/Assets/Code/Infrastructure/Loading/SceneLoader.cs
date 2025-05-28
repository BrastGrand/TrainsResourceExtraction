using Code.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Loading
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IAssetProvider _assetProvider;

        public SceneLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public UniTask LoadScene(string nextScene)
        {
            var sceneHandler = _assetProvider.LoadScene(nextScene);

            if (sceneHandler == null)
            {
                Debug.LogError($"Failed to load scene: {nextScene}");
            }

            return UniTask.CompletedTask;
        }
    }
}