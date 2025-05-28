using Code.Gameplay.Common.UIService;
using Code.Gameplay.UI;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;

namespace Code.Infrastructure.States.GameStates
{
    public class GameEnterState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GameContext _gameContext;
        private readonly IUIFactory _uiFactory;

        private MenuScreen _screen;

        public GameEnterState(IGameStateMachine stateMachine, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
        }

        public override void Enter()
        {

        }
    }
}