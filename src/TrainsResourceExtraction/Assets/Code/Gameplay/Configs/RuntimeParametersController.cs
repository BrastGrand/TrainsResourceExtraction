using UnityEngine;
using Code.Gameplay.Features.Graph;
using Code.Gameplay.Features.Visualization.Systems;
using Zenject;
using System.Collections.Generic;

namespace Code.Gameplay.Configs
{
    /// <summary>
    /// Контроллер для изменения параметров системы в реальном времени через инспектор Unity
    /// </summary>
    public class RuntimeParametersController : MonoBehaviour
    {
        [Header("Базы - Множители ресурсов")]
        [SerializeField] private BaseParameterConfig[] _baseConfigs = new BaseParameterConfig[]
        {
            new BaseParameterConfig { NodeId = 1, ResourceMultiplier = 5.0f }, // База1 - левый верх
            new BaseParameterConfig { NodeId = 2, ResourceMultiplier = 2.0f }, // База2 - центр
            new BaseParameterConfig { NodeId = 3, ResourceMultiplier = 1.0f }    // База3 - ниже центра
        };

        [Header("Шахты - Множители скорости добычи")]
        [SerializeField] private MineParameterConfig[] _mineConfigs = new MineParameterConfig[]
        {
            new MineParameterConfig { NodeId = 4, MiningTimeMultiplier = 0.2f }, // Шахта1 - верхняя правая
            new MineParameterConfig { NodeId = 5, MiningTimeMultiplier = 0.5f }, // Шахта2 - нижняя правая
            new MineParameterConfig { NodeId = 6, MiningTimeMultiplier = 0.9f }, // Шахта3 - нижняя центральная
            new MineParameterConfig { NodeId = 7, MiningTimeMultiplier = 1.0f }  // Шахта4 - нижняя левая
        };

        [Header("Поезда - Скорости")]
        [SerializeField] private TrainParameterConfig[] _trainConfigs = new TrainParameterConfig[]
        {
            new TrainParameterConfig { TrainId = 1, MovementSpeed = 40f, BaseMiningTime = 20f },
            new TrainParameterConfig { TrainId = 2, MovementSpeed = 5f, BaseMiningTime = 1f },
            new TrainParameterConfig { TrainId = 3, MovementSpeed = 10f, BaseMiningTime = 5f }
        };

        [Header("Рёбра графа - Длины")]
        [SerializeField] private EdgeParameterConfig[] _edgeConfigs = new EdgeParameterConfig[]
        {
            // Основные соединения согласно реальному графу
            new EdgeParameterConfig { FromNodeId = 1, ToNodeId = 8, Length = 10f }, // База1 -> Junction1
            new EdgeParameterConfig { FromNodeId = 8, ToNodeId = 9, Length = 20f }, // Junction1 -> Junction2
            new EdgeParameterConfig { FromNodeId = 9, ToNodeId = 4, Length = 40f }, // Junction2 -> Шахта1
            new EdgeParameterConfig { FromNodeId = 4, ToNodeId = 2, Length = 50f }, // Шахта1 -> База2
            new EdgeParameterConfig { FromNodeId = 2, ToNodeId = 3, Length = 10f }, // База2 -> База3
            new EdgeParameterConfig { FromNodeId = 2, ToNodeId = 10, Length = 10f }, // База2 -> Junction3
            new EdgeParameterConfig { FromNodeId = 3, ToNodeId = 10, Length = 10f }, // База3 -> Junction3
            new EdgeParameterConfig { FromNodeId = 10, ToNodeId = 5, Length = 10f }, // Junction3 -> Шахта2
            new EdgeParameterConfig { FromNodeId = 3, ToNodeId = 6, Length = 10f }, // База3 -> Шахта3
            new EdgeParameterConfig { FromNodeId = 3, ToNodeId = 7, Length = 10f }, // База3 -> Шахта4
            new EdgeParameterConfig { FromNodeId = 7, ToNodeId = 8, Length = 50f }    // Шахта4 -> Junction1
        };

        [Inject] private IGraphService _graphService;
        [Inject] private GameContext _gameContext;
        [Inject] private UpdateNodeViewSystem _updateNodeViewSystem;

        private Dictionary<int, BaseNode> _baseNodes = new();
        private Dictionary<int, MineNode> _mineNodes = new();
        private Dictionary<int, GameEntity> _trainEntities = new();

        /// <summary>
        /// Публичный метод для инициализации после создания игрового мира
        /// </summary>
        public void InitializeAfterWorldCreation()
        {
            if (_graphService == null || _gameContext == null)
            {
                return;
            }

            InitializeReferences();
            ApplyAllParameters();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Применяем изменения в реальном времени когда что-то меняется в инспекторе
            // ТОЛЬКО если игра запущена и зависимости инжектированы
            if (Application.isPlaying && _graphService != null && _gameContext != null)
            {
                ApplyAllParameters();
            }
        }
#endif

        private void InitializeReferences()
        {
            foreach (var config in _baseConfigs)
            {
                var rawNode = _graphService.GetNode(config.NodeId);

                var node = rawNode as BaseNode;
                if (node != null)
                {
                    _baseNodes[config.NodeId] = node;
                }
            }

            // Собираем ссылки на шахты
            foreach (var config in _mineConfigs)
            {
                var rawNode = _graphService.GetNode(config.NodeId);

                var node = rawNode as MineNode;
                if (node != null)
                {
                    _mineNodes[config.NodeId] = node;
                }
            }

            RefreshTrainEntitiesCache();
        }

        private void ApplyAllParameters()
        {
            if (_graphService == null || _gameContext == null)
            {
                return;
            }

            ApplyBaseParameters();
            ApplyMineParameters();
            ApplyTrainParameters();
            ApplyEdgeParameters();
            
            ForceRecalculateTrainRoutes();
            ForceUpdateNodeViewUI();
        }

        private void ApplyBaseParameters()
        {
            foreach (var config in _baseConfigs)
            {
                if (_baseNodes.TryGetValue(config.NodeId, out var baseNode))
                {
                    try
                    {
                        baseNode.UpdateResourceMultiplier(config.ResourceMultiplier);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Error updating base {config.NodeId}: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Base node {config.NodeId} not found in cached nodes!");
                }
            }
        }

        private void ApplyMineParameters()
        {
            foreach (var config in _mineConfigs)
            {
                if (_mineNodes.TryGetValue(config.NodeId, out var mineNode))
                {
                    try
                    {
                        mineNode.UpdateMiningTimeMultiplier(config.MiningTimeMultiplier);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Error updating mine {config.NodeId}: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Mine node {config.NodeId} not found in cached nodes!");
                }
            }
        }

        private void ApplyTrainParameters()
        {
            // Обновляем кэш поездов каждый раз при применении параметров
            RefreshTrainEntitiesCache();

            foreach (var config in _trainConfigs)
            {
                if (_trainEntities.TryGetValue(config.TrainId, out var trainEntity))
                {
                    trainEntity.ReplaceSpeed(config.MovementSpeed);
                    trainEntity.ReplaceBaseMiningTime(config.BaseMiningTime);
                }
                else
                {
                    Debug.LogWarning($"Train {config.TrainId} not found in cached entities!");
                }
            }
        }

        private void RefreshTrainEntitiesCache()
        {
            _trainEntities.Clear();
            
            if (_gameContext == null)
            {
                Debug.LogWarning("GameContext is null, cannot refresh train entities cache");
                return;
            }
            
            var trains = _gameContext.GetGroup(GameMatcher.Train);
            if (trains == null) return;

            foreach (var train in trains.GetEntities())
            {
                if (train.hasTrain)
                {
                    _trainEntities[train.Train] = train;
                }
            }
        }

        private void ApplyEdgeParameters()
        {
            foreach (var config in _edgeConfigs)
            {
                var edge = _graphService.GetEdge(config.FromNodeId, config.ToNodeId);
                edge?.UpdateLength(config.Length);
            }
        }

        /// <summary>
        /// Принудительно заставляет поезда пересчитать свои маршруты
        /// Вызывается после изменения параметров узлов или рёбер
        /// </summary>
        private void ForceRecalculateTrainRoutes()
        {
            RefreshTrainEntitiesCache();

            foreach (var trainEntity in _trainEntities.Values)
            {
                // Прерываем текущий маршрут и заставляем выбрать новую цель
                if (ShouldRecalculateRoute(trainEntity))
                {
                    InterruptCurrentRoute(trainEntity);
                }
            }
        }

        /// <summary>
        /// Определяет нужно ли пересчитывать маршрут для данного поезда
        /// </summary>
        private bool ShouldRecalculateRoute(GameEntity trainEntity)
        {
            // Пересчитываем если поезд:
            // 1. Движется к цели (не добывает)
            // 2. Или находится в idle и имеет цель (готовится к движению)
            return (trainEntity.hasTargetNode && !trainEntity.isMining) ||
                   (trainEntity.isIdleState && trainEntity.hasTargetNode);
        }

        /// <summary>
        /// Прерывает текущий маршрут поезда и заставляет выбрать новую цель
        /// </summary>
        private void InterruptCurrentRoute(GameEntity trainEntity)
        {
            trainEntity.isMoving = false;
            
            // Очищаем текущий маршрут
            if (trainEntity.hasTargetDestination)
                trainEntity.RemoveTargetDestination();
                
            if (trainEntity.hasNextNodeInPath)
                trainEntity.RemoveNextNodeInPath();
                
            if (trainEntity.hasTargetNode)
                trainEntity.RemoveTargetNode();
            
            // Помечаем как нуждающегося в новой цели
            trainEntity.isNeedsNewTarget = true;
            trainEntity.isIdleState = true;
        }

        /// <summary>
        /// Принудительно обновляет UI NodeView
        /// </summary>
        private void ForceUpdateNodeViewUI()
        {
            _updateNodeViewSystem?.UpdateAllNodeViews();
        }

        [System.Serializable]
        public class BaseParameterConfig
        {
            public int NodeId;
            public float ResourceMultiplier = 1.0f;
        }

        [System.Serializable]
        public class MineParameterConfig
        {
            public int NodeId;
            public float MiningTimeMultiplier = 1.0f;
        }

        [System.Serializable]
        public class TrainParameterConfig
        {
            public int TrainId;
            public float MovementSpeed = 10f;
            public float BaseMiningTime = 5f;
        }

        [System.Serializable]
        public class EdgeParameterConfig
        {
            public int FromNodeId;
            public int ToNodeId;
            public float Length = 10f;
        }
    }
} 