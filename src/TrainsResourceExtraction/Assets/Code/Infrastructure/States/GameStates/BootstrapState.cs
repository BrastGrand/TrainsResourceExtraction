using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class BootstrapState : SimpleState
    {
        private const string _SCENE = "MenuScene";
        private readonly IGameStateMachine _stateMachine;
        private readonly IAssetProvider _assetProvider;

        public BootstrapState(IGameStateMachine stateMachine, IAssetProvider assetProvider)
        {
            _stateMachine = stateMachine;
            _assetProvider = assetProvider;
        }

        public override async void Enter()
        {
            Debug.Log("[BootstrapState] Enter");

            await _assetProvider.InitializeAsync();
            await _stateMachine.Enter<LoadingMenuState, string>(_SCENE);
        }
    }
}