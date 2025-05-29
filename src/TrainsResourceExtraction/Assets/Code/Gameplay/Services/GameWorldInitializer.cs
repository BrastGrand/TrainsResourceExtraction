using System.Collections.Generic;
using UnityEngine;
using Code.Gameplay.Features.Graph;
using Code.Gameplay.Factories;
using Code.Gameplay.Common.Random;
using Code.Gameplay.Configs;

namespace Code.Gameplay.Services
{
    public class GameWorldInitializer : IGameWorldInitializer
    {
        private readonly IGraphService _graphService;
        private readonly ITrainFactory _trainFactory;
        private readonly IRandomService _randomService;
        private readonly RuntimeParametersController _runtimeParametersController;

        public GameWorldInitializer(IGraphService graphService, ITrainFactory trainFactory, IRandomService randomService, RuntimeParametersController runtimeParametersController)
        {
            _graphService = graphService;
            _trainFactory = trainFactory;
            _randomService = randomService;
            _runtimeParametersController = runtimeParametersController;
        }

        public void InitializeWorld()
        {
            Debug.Log("Initializing game world...");
            CreateGraph();
            CreateTrains();

            Debug.Log("Initializing RuntimeParametersController...");
            _runtimeParametersController.InitializeAfterWorldCreation();
        }

        private void CreateGraph()
        {
            // БАЗЫ
            var base1 = new BaseNode(1, new Vector3(-34, 14, 0), 5.0f);   // 5x - левый верх
            var base2 = new BaseNode(2, new Vector3(-5.2f, 2.6f, 0), 2f);    // 2x - центр
            var base3 = new BaseNode(3, new Vector3(-9, -10, 0), 1.0f);  // 1x - ниже центра

            // ШАХТЫ
            var mine1 = new MineNode(4, new Vector3(17, 17, 0), 0.2f);      // 0.2x - верхняя правая шахта
            var mine2 = new MineNode(5, new Vector3(10, -21, 0), 0.5f);     // 0.5x - нижняя правая шахта
            var mine3 = new MineNode(6, new Vector3(-10, -21, 0), 0.9f);     // 0.9x - нижняя центральная шахта
            var mine4 = new MineNode(7, new Vector3(-30, -18, 0), 1f);     // 1x - нижняя левая шахта

            // ПРОМЕЖУТОЧНЫЕ УЗЛЫ
            var junction1 = new EmptyNode(8, new Vector3(-22, 7, 0));      // левый верхний узел
            var junction2 = new EmptyNode(9, new Vector3(-4, 12, 0));      // центральный верхний узел
            var junction3 = new EmptyNode(10, new Vector3(10, -12, 0));      // правый нижний узел

            // Добавляем узлы в граф
            _graphService.AddNode(base1);
            _graphService.AddNode(base2);
            _graphService.AddNode(base3);
            _graphService.AddNode(mine1);
            _graphService.AddNode(mine2);
            _graphService.AddNode(mine3);
            _graphService.AddNode(mine4);
            _graphService.AddNode(junction1);
            _graphService.AddNode(junction2);
            _graphService.AddNode(junction3);

            // Основные соединения от баз к промежуточным узлам
            _graphService.AddEdge(new GraphEdge(base1, junction1, 10f));
            _graphService.AddEdge(new GraphEdge(junction1, base1, 10f));

            _graphService.AddEdge(new GraphEdge(junction1, junction2, 20f));
            _graphService.AddEdge(new GraphEdge(junction2, junction1, 20f));

            _graphService.AddEdge(new GraphEdge(junction2, mine1, 40f));
            _graphService.AddEdge(new GraphEdge(mine1, junction2, 40f));

            _graphService.AddEdge(new GraphEdge(mine1, base2, 50f));
            _graphService.AddEdge(new GraphEdge(base2, mine1, 50f));

            _graphService.AddEdge(new GraphEdge(base2, base3, 10f));
            _graphService.AddEdge(new GraphEdge(base3, base2, 10f));

            _graphService.AddEdge(new GraphEdge(base2, junction3, 10f));
            _graphService.AddEdge(new GraphEdge(junction3, base2, 10f));

            _graphService.AddEdge(new GraphEdge(base3, junction3, 10f));
            _graphService.AddEdge(new GraphEdge(junction3, base3, 10f));

            _graphService.AddEdge(new GraphEdge(junction3, mine2, 10f));
            _graphService.AddEdge(new GraphEdge(mine2, junction3, 10f));

            _graphService.AddEdge(new GraphEdge(base3, mine3, 10f));
            _graphService.AddEdge(new GraphEdge(mine3, base3, 10f));

            _graphService.AddEdge(new GraphEdge(base3, mine4, 10f));
            _graphService.AddEdge(new GraphEdge(mine4, base3, 10f));

            _graphService.AddEdge(new GraphEdge(mine4, junction1, 50f));
            _graphService.AddEdge(new GraphEdge(junction1, mine4, 50f));

        }

        public void CreateTrains()
        {
            var allNodes = _graphService.GetAllNodes();
            Debug.Log($"Creating trains. Available nodes: {allNodes.Count}");
            
            // Создаем копию списка узлов для перемешивания
            var availableNodes = new List<GraphNode>(allNodes);
            
            // Перемешиваем список чтобы получить случайный порядок
            ShuffleList(availableNodes);
            
            // Берем первые 3 узла из перемешанного списка - гарантированно разные
            var spawnNode1 = availableNodes[0];
            var spawnNode2 = availableNodes[1];
            var spawnNode3 = availableNodes[2];
            
            // Поезд 1: Скорость 40, Время добычи 20s
            var train1 = _trainFactory.CreateTrain(1, 40f, 20f, spawnNode1.Position, spawnNode1.Id);
            train1.isIdleState = true;
            Debug.Log($"Created Train 1 at node {spawnNode1.Id} ({spawnNode1.GetNodeType()}), position {spawnNode1.Position}");
            
            // Поезд 2: Скорость 5, Время добычи 1s
            var train2 = _trainFactory.CreateTrain(2, 5f, 1f, spawnNode2.Position, spawnNode2.Id);
            train2.isIdleState = true;
            Debug.Log($"Created Train 2 at node {spawnNode2.Id} ({spawnNode2.GetNodeType()}), position {spawnNode2.Position}");

            // Поезд 3: Скорость 10, Время добычи 5s
            var train3 = _trainFactory.CreateTrain(3, 10f, 5f, spawnNode3.Position, spawnNode3.Id);
            train3.isIdleState = true;
            Debug.Log($"Created Train 3 at node {spawnNode3.Id} ({spawnNode3.GetNodeType()}), position {spawnNode3.Position}");
        }

        /// <summary>
        /// Перемешивает список используя алгоритм Фишера-Йетса
        /// </summary>
        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = _randomService.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        public void CreateGraphFromImage(Texture2D graphImage)
        {
            // Реализация для создания графа из изображения
        }
    }
} 