using UnityEngine;

namespace Code.Gameplay.Services
{
    public class SceneHierarchyService : ISceneHierarchyService
    {
        private Transform _levelObjectsContainer;
        private Transform _trainContainer;
        private Transform _nodesContainer;
        private Transform _systemObjectsContainer;

        public Transform GetLevelObjectsContainer()
        {
            if (_levelObjectsContainer == null)
            {
                var levelObjects = GameObject.Find("LevelObjects");
                if (levelObjects == null)
                {
                    levelObjects = new GameObject("LevelObjects");
                    Debug.Log("SceneHierarchyService: Created LevelObjects container");
                }
                _levelObjectsContainer = levelObjects.transform;
            }
            return _levelObjectsContainer;
        }

        public Transform GetTrainContainer()
        {
            if (_trainContainer == null)
            {
                var levelObjects = GetLevelObjectsContainer();
                var trainContainer = levelObjects.Find("Trains");
                if (trainContainer == null)
                {
                    var trainsObject = new GameObject("Trains");
                    trainsObject.transform.SetParent(levelObjects);
                    trainContainer = trainsObject.transform;
                    Debug.Log("SceneHierarchyService: Created Trains container");
                }
                _trainContainer = trainContainer;
            }
            return _trainContainer;
        }

        public Transform GetNodesContainer()
        {
            if (_nodesContainer == null)
            {
                var levelObjects = GetLevelObjectsContainer();
                var nodesContainer = levelObjects.Find("Nodes");
                if (nodesContainer == null)
                {
                    var nodesObject = new GameObject("Nodes");
                    nodesObject.transform.SetParent(levelObjects);
                    nodesContainer = nodesObject.transform;
                    Debug.Log("SceneHierarchyService: Created Nodes container");
                }
                _nodesContainer = nodesContainer;
            }
            return _nodesContainer;
        }

        public Transform GetSystemObjectsContainer()
        {
            if (_systemObjectsContainer == null)
            {
                var systemObjects = GameObject.Find("SystemObjects");
                if (systemObjects == null)
                {
                    systemObjects = new GameObject("SystemObjects");
                    Debug.Log("SceneHierarchyService: Created SystemObjects container");
                }
                _systemObjectsContainer = systemObjects.transform;
            }
            return _systemObjectsContainer;
        }
    }
} 