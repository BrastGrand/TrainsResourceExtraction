using Code.Gameplay.Common.Random;
using Code.Gameplay.Common.Time;
using Code.Gameplay.Common.UIService;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Identifiers;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Infrastructure.Systems;
using Code.Gameplay.Features.Graph;
using Code.Gameplay.Features.ResourceManagement.Systems;
using Code.Gameplay.Features.Mining.Systems;
using Code.Gameplay.Features.TrainAI.Systems;
using Code.Gameplay.Factories;
using Code.Gameplay.Services;
using Code.Gameplay.Configs;
using Code.Gameplay.Features.Visualization;
using Code.Gameplay.Features.Visualization.Systems;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            BindInfrastructureServices();
            BindAssetManagementServices();
            BindCommonServices();
            BindSystemFactory();
            BindUIServices();
            BindContexts();
            BindFeatureServices();
            BindRuntimeParameters();
            BindGameplayFactories();
            BindGameplaySystems();
            BindStateMachine();
            BindStateFactory();
            BindGameStates();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>().Enter<BootstrapState>();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }

        private void BindStateFactory()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuEnterState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameEnterState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameLoopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingMenuState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingGameState>().AsSingle();
        }

        private void BindContexts()
        {
            Container.Bind<Contexts>().FromInstance(Contexts.sharedInstance).AsSingle();
            Container.Bind<GameContext>().FromInstance(Contexts.sharedInstance.game).AsSingle();
        }

        private void BindFeatureServices()
        {
            Container.Bind<IGraphService>().To<GraphService>().AsSingle();
            Container.Bind<IUIService>().To<UIService>().AsSingle();
            Container.Bind<IGameWorldInitializer>().To<GameWorldInitializer>().AsSingle();
            Container.Bind<ISceneHierarchyService>().To<SceneHierarchyService>().AsSingle();
        }

        private void BindGameplayFactories()
        {
            Container.Bind<ITrainFactory>().To<TrainFactory>().AsSingle();
        }

        private void BindRuntimeParameters()
        {
            // Контроллер runtime параметров - регистрируем ДО GameWorldInitializer
            Container.Bind<RuntimeParametersController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void BindGameplaySystems()
        {
            // ResourceManagement Systems
            Container.Bind<UpdateTotalResourcesSystem>().AsSingle();
            Container.Bind<UpdateResourcesUISystem>().AsSingle();
            
            // Mining Systems
            Container.Bind<ProcessMiningSystem>().AsSingle();
            Container.Bind<CompleteMiningSystem>().AsSingle();
            
            // TrainAI Systems
            Container.Bind<InitializeTrainAISystem>().AsSingle();
            Container.Bind<TrainStateManagerSystem>().AsSingle();
            Container.Bind<SelectTargetNodeSystem>().AsSingle();
            Container.Bind<StartMovementToTargetSystem>().AsSingle();
            
            // Visualization Registry
            Container.Bind<NodeViewRegistry>().AsSingle();
            
            // Visualization Systems
            Container.Bind<CreateNodeViewSystem>().AsSingle();
            Container.Bind<CreateTrainViewSystem>().AsSingle();
            Container.Bind<CreateTrainViewReactiveSystem>().AsSingle();
            Container.Bind<UpdateTrainViewSystem>().AsSingle();
            Container.Bind<UpdateNodeViewSystem>().AsSingle();
            Container.Bind<GraphVisualizationSystem>().AsSingle();
        }

        private void BindSystemFactory()
        {
            Container.Bind<ISystemFactory>().To<SystemFactory>().AsSingle();
        }

        private void BindInfrastructureServices()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
            Container.Bind<IIdentifierService>().To<IdentifierService>().AsSingle();
        }

        private void BindAssetManagementServices()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        }

        private void BindCommonServices()
        {
            Container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            Container.Bind<ITimeService>().To<UnityTimeService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }

        private void BindUIServices()
        {
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}