using Assets.Scripts.Infrastructure.Services.PauseAndContinue;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.UserInterface;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IGameUI _gameUI;
        private readonly IPauseContinueService _pauseContinueService;

        public GameLoopState(GameStateMachine stateMachine,
            IGameUI gameUI,
            IPauseContinueService pauseContinueService)
        {
            _stateMachine = stateMachine;
            _gameUI = gameUI;
            _pauseContinueService = pauseContinueService;
        }

        public void Enter()
        {
            _gameUI.OpenScreen(ScreenID.Gameplay);

            if (_pauseContinueService.IsPaused)
            {
                _pauseContinueService.Continue();
            }
        }

        public void Exit()
        {
            if (_pauseContinueService.IsPaused)
            {
                _pauseContinueService.Continue();
            }
        }
    }
}