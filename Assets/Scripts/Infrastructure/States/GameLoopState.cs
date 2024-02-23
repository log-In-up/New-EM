using Assets.Scripts.UserInterface;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IGameUI _gameUI;

        public GameLoopState(GameStateMachine stateMachine, IGameUI gameUI)
        {
            _stateMachine = stateMachine;
            _gameUI = gameUI;
        }

        public void Enter()
        {
            _gameUI.OpenScreen(WindowID.Gameplay);
        }

        public void Exit()
        {
        }
    }
}