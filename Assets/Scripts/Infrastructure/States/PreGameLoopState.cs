namespace Assets.Scripts.Infrastructure.States
{
    public class PreGameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public PreGameLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
            //_stateMachine.Enter<LoadLevelState, string>(_progressService.GameData.CurrentLevel);
        }
    }
}