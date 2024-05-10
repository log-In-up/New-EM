using Assets.Scripts.Infrastructure.Services.PauseAndContinue;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly IGameUI _gameUI;
        private readonly IPauseContinueService _pauseContinueService;
        private readonly IGameStateMachine _stateMachine;

        public GameLoopState(IGameStateMachine stateMachine,
            IGameUI gameUI,
            IPauseContinueService pauseContinueService)
        {
            _stateMachine = stateMachine;
            _gameUI = gameUI;
            _pauseContinueService = pauseContinueService;
        }

        public void Enter()
        {
            Application.focusChanged += OnChangeFocus;

            _gameUI.OpenScreen(ScreenID.Gameplay);
            UnpauseGame();
        }

        public void Exit()
        {
            Application.focusChanged -= OnChangeFocus;

            UnpauseGame();
        }

        private void OnChangeFocus(bool value)
        {
            if (_pauseContinueService.IsPaused) return;

            _pauseContinueService.Pause();
            _gameUI.OpenScreen(ScreenID.GamePauseScreen);
        }

        private void UnpauseGame()
        {
            if (_pauseContinueService.IsPaused)
            {
                _pauseContinueService.Continue();
            }
        }
    }
}