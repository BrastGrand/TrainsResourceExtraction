# Trains Resource Extraction

## Описание проекта

**Trains Resource Extraction** — это симуляционная игра на Unity, с системой автономных поездов, которые добывают ресурсы из шахт и доставляют их на базы через сложный граф маршрутов.


## Основные особенности

- **Полноценная ECS-архитектура** с использованием Entitas
- **Умный ИИ поездов** с оптимизацией маршрутов
- **Динамические параметры** в реальном времени через Inspector
- **Визуализация графа** и путей через Gizmos
- **Модульная архитектура** с четким разделением ответственности
- **Реактивные системы** для производительности
- **UI система** с реальным отслеживанием ресурсов

## Архитектура проекта

### ECS (Entity-Component-System)

Проект построен на основе **чистой ECS-архитектуры**:

#### **Entities (Сущности)**
```csharp
GameEntity train = CreateEntity.Empty()
    .AddTrain(1)
    .AddSpeed(40f)
    .AddCurrentNode(nodeId)
    .AddIdleState();
```

#### **Components (Компоненты)**
```csharp
[Game] public class Speed : IComponent { public float Value; }
[Game] public class Direction : IComponent { public Vector2 Value; }
[Game] public class Moving : IComponent { } // Flag component
```

#### **Systems (Системы)**
```csharp
public class DirectionalDeltaMoveSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _movers;
    
    public void Execute()
    {
        foreach (GameEntity entity in _movers)
        {
            // Обновляем позицию
            var newPosition = entity.WorldPosition + 
                entity.Direction * entity.Speed * _time.DeltaTime;
            entity.ReplaceWorldPosition(newPosition);
        }
    }
}
```

### Структура проекта

```
Assets/Code/
├── Common/                     # Общие компоненты и сервисы
│   ├── Entity/                 # Фабрика сущностей
│   ├── Random/                 # Сервисы рандома
│   ├── Time/                   # Сервисы времени
│   └── UIService/              # UI сервисы
├── Gameplay/                   # Игровая логика
│   ├── Features/               # Модульные фичи
│   │   ├── Graph/             # Граф маршрутов
│   │   ├── Movement/          # Система движения
│   │   ├── Mining/            # Добыча ресурсов
│   │   ├── ResourceManagement/ # Управление ресурсами
│   │   ├── TrainAI/           # ИИ поездов
│   │   ├── Visualization/     # Визуализация
│   │   └── TargetCollection/  # Сбор целей
│   ├── Factories/             # Фабрики объектов
│   ├── Services/              # Доменные сервисы
│   └── Configs/               # Конфигурации
├── Infrastructure/            # Инфраструктурный слой
│   ├── AssetManagement/       # Управление ресурсами
│   ├── Identifiers/           # Сервисы ID
│   ├── Loading/               # Загрузка сцен
│   ├── States/               # Машина состояний
│   └── Systems/              # Фабрика систем
└── UI/                       # Пользовательский интерфейс
```

## Игровые системы

### Система поездов

#### **Компоненты поездов:**
- `Train` - ID поезда
- `Speed` - скорость движения
- `CurrentNode` - текущий узел
- `TargetNode` - целевой узел
- `NextNodeInPath` - следующий узел в пути
- `BaseMiningTime` - базовое время добычи

#### **Состояния поездов:**
- **Idle** - ожидание команд
- **SelectingTarget** - выбор оптимальной цели
- **Moving** - движение к цели
- **InTransit** - движение к промежуточному узлу
- **Mining** - добыча ресурсов
- **HasResource** - везёт ресурс на базу

### Система графа

#### **Типы узлов:**
```csharp
public enum NodeType
{
    Base,    // База - принимает ресурсы
    Mine,    // Шахта - источник ресурсов
    Empty    // Пустой узел - транзитный
}
```

#### **Граф маршрутов:**
```
    Base1(5x) ——— EmptyNode8 ——— Mine4(0.5x)
       |             |              |
    Base2(2x) ——— EmptyNode9 ——— Mine5(1.0x)
       |             |              |
    Base3(1x) ——— EmptyNode10 —— Mine6(2.0x)
                     |
                  Mine7(1.5x)
```

### ИИ система

#### **Умный выбор цели:**
```csharp
// Для шахт: эффективность = 1 / (время_добычи + время_пути)
float mineEfficiency = 1f / (miningTime + travelTime);

// Для баз: эффективность = множитель_ресурсов / время_пути  
float baseEfficiency = resourceMultiplier / travelTime;
```

#### **Алгоритм поиска пути:**
- Использует алгоритм Дейкстры для поиска кратчайшего пути
- Двунаправленные рёбра для свободного движения
- Динамический пересчёт при изменении параметров

### Система параметров

#### **RuntimeParametersController:**
- **Реальное время изменений** через Unity Inspector
- **Автоматическое применение** к существующим объектам
- **Принудительный пересчёт** маршрутов при изменениях

#### **Конфигурации:**
```csharp
[System.Serializable]
public class BaseConfig
{
    public int NodeId;
    public float ResourceMultiplier;
}

[System.Serializable]  
public class TrainConfig
{
    public int TrainId;
    public float Speed;
    public float MiningTime;
}
```

## Архитектурные принципы

### SOLID Principles

#### **S - Single Responsibility:**
```csharp
public class DirectionalDeltaMoveSystem : IExecuteSystem
{
    // Отвечает ТОЛЬКО за движение по направлению
}
```

#### **O - Open/Closed:**
```csharp
public abstract class GraphNode
{
    // Закрыт для изменений, открыт для расширения
    public abstract NodeType GetNodeType();
}
```

#### **L - Liskov Substitution:**
```csharp
GraphNode node = new BaseNode(1, Vector3.zero, 2.0f);
// Любой наследник корректно заменяет базовый класс
```

#### **I - Interface Segregation:**
```csharp
public interface IGraphService { ... }      // Только граф
public interface IMovementService { ... }   // Только движение
public interface ISceneHierarchyService { ... } // Только иерархия
```

#### **D - Dependency Inversion:**
```csharp
public TrainStateManagerSystem(
    GameContext context, 
    IGraphService graphService) // Зависимость от абстракции
```

### Паттерны проектирования

#### **Factory Pattern:**
```csharp
public interface ITrainFactory
{
    GameEntity CreateTrain(int trainId, float speed, 
        float miningTime, Vector3 position, int startNodeId);
}
```

#### **Service Locator (через DI):**
```csharp
// Zenject Container
Container.Bind<IGraphService>().To<GraphService>().AsSingle();
```

#### **Observer Pattern (через ECS):**
```csharp
// Реактивные системы автоматически реагируют на изменения компонентов
protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
{
    return context.CreateCollector(GameMatcher.Train.Added());
}
```

#### **State Machine:**
```csharp
// TrainStateManagerSystem управляет состояниями поездов
private void ProcessTrainState(GameEntity entity)
{
    if (entity.isReached) HandleNodeArrival(entity);
    else if (entity.isMoving) return;
    else if (entity.isMining) return;
    // ... другие состояния
}
```

## Визуализация

### Gizmos система
- **Граф маршрутов** - серые линии между узлами
- **Пути поездов** - цветные линии до целей
- **Текущее движение** - анимированные точки
- **Направления** - стрелки на рёбрах

### Организация сцены
```
Game Scene
├── LevelObjects/          ← Все игровые объекты
│   ├── Trains/           ← Все поезда
│   │   ├── Train_1
│   │   ├── Train_2
│   │   └── Train_3
│   └── Nodes/            ← Все узлы графа
│       ├── Base_1
│       ├── Mine_4
│       └── EmptyNode_8
├── SystemObjects/         ← Системные объекты
│   └── GraphVisualization
└── Canvas                ← UI элементы
    └── ResourcesUI
```

## Features Architecture

### GameplayFeature
```csharp
public sealed class GameplayFeature : Feature
{
    public GameplayFeature(ISystemFactory systems)
    {
        Add(new MovementFeature(systems));
        Add(new MiningFeature(systems));
        Add(new ResourceManagementFeature(systems));
        Add(new TrainAIFeature(systems));
    }
}
```

### VisualizationFeature
```csharp
public class VisualizationFeature : Feature
{
    public VisualizationFeature(ISystemFactory systems)
    {
        Add(systems.Create<CreateNodeViewSystem>());
        Add(systems.Create<UpdateTrainViewSystem>());
        Add(systems.Create<GraphVisualizationSystem>());
    }
}
```

## Запуск проекта

### Требования
- **Unity 6000.0.47** или новее
- **Entitas Framework**
- **Zenject** (Extenject)
- **Addressables**
- **TextMeshPro**

### Настройка

1. **Клонирование:**
```bash
git clone <repository-url>
cd TrainsResourceExtraction
```

2. **Открытие в Unity:**
   - Откройте проект в Unity
   - Дождитесь импорта зависимостей

3. **Настройка Addressables:**
   - `Window → Asset Management → Addressables → Groups`
   - `Build → New Build → Default Build Script`

4. **Запуск:**
   - Откройте сцену `Assets/Scenes/Bootstrap.unity`
   - Нажмите Play

### Управление

- **Просмотр графа:** Scene View с включенными Gizmos
- **Изменение параметров:** Inspector → RuntimeParametersController (ProjectContext)
- **Мониторинг ресурсов:** Game View → левый верхний угол

## Производительность

### ECS Оптимизации
- **Группы сущностей** для быстрой фильтрации
- **Реактивные системы** только при изменениях
- **Буферизация** списков для избежания аллокаций
- **Object pooling** для временных объектов

### Архитектурные оптимизации
- **Lazy initialization** сервисов
- **Dependency Injection** для эффективного управления зависимостями
- **Feature-based organization** для модульности

### Профилирование
- **Unity Profiler** для мониторинга производительности
- **Entitas Visual Debugging** для отслеживания систем
- **Deep Profiling** в Development Build

## 🔧 Расширение системы

### ➕ Добавление нового типа узла
```csharp
public class WarehouseNode : GraphNode
{
    public override NodeType GetNodeType() => NodeType.Warehouse;
    public override float GetMultiplier() => StorageCapacity;
}
```

### ➕ Добавление новой системы
```csharp
public class NewFeatureSystem : IExecuteSystem
{
    public void Execute()
    {
        // Логика новой фичи
    }
}
```

### ➕ Регистрация в DI
```csharp
// В BootstrapInstaller.cs
Container.Bind<NewFeatureSystem>().AsSingle();
```

## Используемые технологии

- **[Unity 6000.0.47](https://unity.com/)** - игровой движок
- **[Entitas](https://github.com/sschmid/Entitas-CSharp)** - ECS framework
- **[Zenject](https://github.com/modesttree/Zenject)** - Dependency Injection
- **[Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest/)** - управление ресурсами
- **[UniTask](https://github.com/Cysharp/UniTask)** - асинхронное программирование