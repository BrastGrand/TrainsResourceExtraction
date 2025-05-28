using System;
using Code.Gameplay.Common.UIService;
using Code.Gameplay.UI;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class LoadingGameState : SimplePayloadState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        private IScreen _screen;

        private const float _MIN_LOADING_TIME = 1.5f;

        public LoadingGameState(IGameStateMachine stateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public override async UniTask Enter(string sceneName)
        {
            _screen = await _uiFactory.CreateScreen<LoadingScreen>("LoadingScreen");
            await _screen.Show();

            var sceneLoadingTask = _sceneLoader.LoadScene(sceneName);
            var minTimeTask = UniTask.Delay(TimeSpan.FromSeconds(_MIN_LOADING_TIME));

            await UniTask.WhenAll(sceneLoadingTask, minTimeTask);
            _stateMachine.Enter<GameEnterState>();
        }

        protected override async void Exit()
        {
            base.Exit();

            if (_sceneLoader != null)
            {
                await _screen.Hide();
                _uiFactory.CloseScreen<LoadingScreen>();
            }
        }
    }
}