using Code.Gameplay;
using Code.Gameplay.Features.Visualization;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.Systems;

namespace Code.Infrastructure.States.GameStates
{
    public class GameLoopState : SimpleState
    {
        private readonly ISystemFactory _systemFactory;
        private Entitas.Systems _systems;

        public GameLoopState(ISystemFactory systemFactory)
        {
            _systemFactory = systemFactory;
        }

        public override void Enter()
        {
            CreateSystems();
            InitializeSystems();
        }

        private void CreateSystems()
        {
            _systems = new Entitas.Systems()
                .Add(new GameplayFeature(_systemFactory))
                .Add(new VisualizationFeature(_systemFactory));
        }

        private void InitializeSystems()
        {
            _systems.Initialize();
        }

        public override void Update()
        {
            _systems?.Execute();
            _systems?.Cleanup();
        }

        protected override void Exit()
        {
            _systems?.TearDown();
            _systems = null;
        }
    }
}