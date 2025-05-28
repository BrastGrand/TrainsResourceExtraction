using Code.Gameplay.Common.UIService;
using Code.Gameplay.UI;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;

namespace Code.Infrastructure.States.GameStates
{
    public class MenuEnterState : SimpleState
    {
        private const string _SCENE = "GameScene";
        private readonly IGameStateMachine _stateMachine;
        private readonly GameContext _gameContext;
        private readonly IUIFactory _uiFactory;

        private MenuScreen _screen;

        public MenuEnterState(IGameStateMachine stateMachine, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
        }

        public override async void Enter()
        {
            //в идеале, нужно доработать CreateScreen, чтобы название префаба бралось автоматически из названия класса
            _screen = await _uiFactory.CreateScreen<MenuScreen>("MenuScreen");
            _screen.OnStartGameClicked += OnStartGameClicked;
            await _screen.Show();
        }

        private async void OnStartGameClicked()
        {
            await _stateMachine.Enter<LoadingGameState, string>(_SCENE);
            await _screen.Hide();
            _uiFactory.CloseScreen<MenuScreen>();
        }
    }
}