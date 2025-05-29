# 🎮 Инструкция по настройке сцены Game

## Шаг 1: Создание префабов в Unity Editor

### 1.1 Создание префаба TrainView
1. В Unity Editor создайте пустой GameObject: `GameObject → Create Empty`
2. Назовите его `TrainView`
3. Добавьте компонент `SpriteRenderer`:
   - Sprite: `Knob` (встроенный Unity спрайт)
   - Color: `(0.2, 0.8, 0.2, 1)` - зеленый
   - Size: `(0.3, 0.3)`
   - Sorting Order: `1`
4. Добавьте компонент `TrainView` (скрипт уже создан)
5. Создайте дочерние объекты для текста:
   - **TrainIdText**: GameObject с `TextMeshPro - Text (3D)`
     - Text: "T1"
     - Font Size: 2
     - Position: (0, 0.2, 0)
     - Color: White
   - **SpeedText**: GameObject с `TextMeshPro - Text (3D)`
     - Text: "200"
     - Font Size: 1.5
     - Position: (0, -0.2, 0)
     - Color: Yellow
   - **StatusText**: GameObject с `TextMeshPro - Text (3D)`
     - Text: "Idle"
     - Font Size: 1
     - Position: (0, -0.4, 0)
     - Color: White
6. Перетащите объект в папку `Assets/Prefabs/Gameplay/`
7. Удалите объект из сцены

### 1.2 Создание префаба BaseNode
1. Создайте пустой GameObject: `BaseNode`
2. Добавьте `SpriteRenderer`:
   - Sprite: `Knob`
   - Color: `(0.5, 0.8, 1, 1)` - голубой
   - Size: `(0.5, 0.5)`
3. Добавьте компонент `NodeView` (скрипт уже создан)
4. Создайте дочерние текстовые объекты:
   - **NodeIdText**: "N1", размер 2, позиция (0, 0.3, 0)
   - **MultiplierText**: "x1.0", размер 1.5, позиция (0, -0.3, 0), цвет желтый
5. Сохраните как префаб в `Assets/Prefabs/Gameplay/`

### 1.3 Создание префаба MineNode
1. Создайте пустой GameObject: `MineNode`
2. Добавьте `SpriteRenderer`:
   - Sprite: `Knob`
   - Color: `(0.8, 0.2, 0.8, 1)` - фиолетовый
   - Size: `(0.5, 0.5)`
3. Добавьте компонент `NodeView`
4. Создайте дочерние текстовые объекты аналогично BaseNode
5. Сохраните как префаб

## Шаг 2: Настройка Addressables

### 2.1 Добавление префабов в Addressables
1. Откройте `Window → Asset Management → Addressables → Groups`
2. Выберите группу `Default Local Group`
3. Перетащите префабы в группу:
   - `TrainView.prefab` → адрес: `"TrainView"`
   - `BaseNode.prefab` → адрес: `"BaseNode"`
   - `MineNode.prefab` → адрес: `"MineNode"`

### 2.2 Сборка Addressables
1. В окне Addressables Groups нажмите `Build → New Build → Default Build Script`
2. Дождитесь завершения сборки

## Шаг 3: Настройка сцены Game

### 3.1 Настройка камеры
1. Откройте сцену `Assets/Scenes/Game.unity`
2. Выберите Main Camera:
   - Position: `(0, 0, -10)`
   - Projection: `Orthographic`
   - Size: `8`
   - Background: `(0.1, 0.1, 0.1, 1)` - темно-серый

### 3.2 Создание UI для отображения ресурсов
1. **Создайте Canvas**: `GameObject → UI → Canvas`
2. **Настройте Canvas**:
   - Render Mode: `Screen Space - Overlay`
   - Canvas Scaler: `Scale With Screen Size`
   - Reference Resolution: `(1920, 1080)`

3. **Создайте GameObject для UI ресурсов**:
   - Создайте дочерний объект Canvas: `GameObject → Create Empty`
   - Назовите его `ResourcesUI`
   - Добавьте компонент `ResourcesDisplay` (скрипт уже создан)

4. **Создайте текстовые элементы**:
   
   **a) Total Resources Text:**
   - Создайте дочерний объект: `GameObject → UI → Text - TextMeshPro`
   - Назовите `TotalResourcesText`
   - Настройте RectTransform:
     - Anchor: Top-Left
     - Position: (20, -20, 0)
     - Size: (300, 50)
   - Настройте TextMeshPro:
     - Text: "Total Resources: 0"
     - Font Size: 24
     - Color: White
     - Alignment: Left

   **b) Delivered Resources Text:**
   - Создайте дочерний объект: `GameObject → UI → Text - TextMeshPro`
   - Назовите `DeliveredResourcesText`
   - Настройте RectTransform:
     - Anchor: Top-Left
     - Position: (20, -70, 0)
     - Size: (300, 50)
   - Настройте TextMeshPro:
     - Text: "Last Delivery: +0"
     - Font Size: 18
     - Color: Yellow
     - Alignment: Left

5. **Связывание компонентов**:
   - Выберите объект `ResourcesUI`
   - В компоненте `ResourcesDisplay`:
     - Перетащите `TotalResourcesText` в поле `Total Resources Text`
     - Перетащите `DeliveredResourcesText` в поле `Delivered Resources Text`

## Шаг 4: Обновление BootstrapInstaller

Добавьте в `BootstrapInstaller.cs` регистрацию новых систем:

```csharp
private void BindGameplaySystems()
{
    // ResourceManagement Systems
    Container.Bind<ProcessResourceDeliverySystem>().AsSingle();
    Container.Bind<UpdateTotalResourcesSystem>().AsSingle();
    Container.Bind<UpdateResourcesUISystem>().AsSingle(); // Новая система
    
    // ... existing systems ...
    
    // Visualization Systems
    Container.Bind<CreateTrainViewSystem>().AsSingle();
    Container.Bind<UpdateTrainViewSystem>().AsSingle();
    Container.Bind<CreateNodeViewSystem>().AsSingle();
}
```

## Шаг 5: Добавление VisualizationFeature в GameLoopState

В `GameLoopState.cs` добавьте:

```csharp
private void CreateSystems()
{
    _systems = new Systems()
        .Add(new GameplayFeature(_systemFactory))
        .Add(new VisualizationFeature(_systemFactory));
}
```

## Шаг 6: Тестирование

1. Запустите игру
2. Перейдите в сцену Game через меню
3. Вы должны увидеть:
   - **В левом верхнем углу**: UI с общими ресурсами
   - **На сцене**: 5 баз (голубые круги) с множителями
   - **На сцене**: 2 шахты (фиолетовые круги) с множителями
   - **На сцене**: 2 поезда (зеленые круги) с ID и статусами
   - **Поведение**: Поезда начинают движение и добычу ресурсов
   - **UI обновления**: При доставке ресурсов UI автоматически обновляется

## Как работает UI система:

1. **UpdateResourcesUISystem** отслеживает изменения в компонентах `TotalResources` и `ResourceDelivered`
2. При изменении ресурсов система автоматически находит `ResourcesDisplay` на сцене
3. `ResourcesDisplay` обновляет текстовые элементы с новыми значениями
4. UI показывает:
   - Общее количество ресурсов
   - Последнюю доставку ресурсов

## Возможные проблемы и решения

### Проблема: UI не обновляется
**Решение**: Убедитесь, что:
- Компонент `ResourcesDisplay` добавлен на объект в сцене
- Текстовые поля правильно связаны в инспекторе
- Система `UpdateResourcesUISystem` зарегистрирована в DI

### Проблема: Текст не отображается
**Решение**: 
- Убедитесь, что TextMeshPro импортирован
- Проверьте настройки Canvas и RectTransform
- Убедитесь, что шрифт TextMeshPro назначен

### Проблема: Префабы не загружаются
**Решение**: Убедитесь, что:
- Префабы добавлены в Addressables с правильными адресами
- Выполнена сборка Addressables
- Адреса в коде совпадают с адресами в Addressables

## Дополнительные улучшения

1. **Анимация UI**: Добавьте анимацию при изменении ресурсов
2. **Звуковые эффекты**: Добавьте звук при доставке ресурсов
3. **Детальная статистика**: Показывайте ресурсы по типам баз
4. **Графики**: Добавьте график добычи ресурсов во времени
5. **Панели управления**: Создайте UI для изменения параметров поездов в runtime 