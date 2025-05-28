using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.UI
{
    public class MenuScreen : BaseScreen
    {
        [SerializeField] private Button _startGameButton;

        public event Action OnStartGameClicked;

        protected override void Awake()
        {
            base.Awake();
            _startGameButton.onClick.AddListener(StartGameClickHandler);
        }

        private void StartGameClickHandler()
        {
            OnStartGameClicked?.Invoke();
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(StartGameClickHandler);
        }
    }
}