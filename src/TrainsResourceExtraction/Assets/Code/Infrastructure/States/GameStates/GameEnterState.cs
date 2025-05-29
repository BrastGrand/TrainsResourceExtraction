using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Code.Gameplay.Services;

namespace Code.Infrastructure.States.GameStates
{
    public class GameEnterState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameWorldInitializer _worldInitializer;

        public GameEnterState(IGameStateMachine stateMachine, IGameWorldInitializer worldInitializer)
        {
            _stateMachine = stateMachine;
            _worldInitializer = worldInitializer;
        }

        public override void Enter()
        {
            _worldInitializer.InitializeWorld();
            _stateMachine.Enter<GameLoopState>();
        }
    }
}