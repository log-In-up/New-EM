using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.UserInterface;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameUI _gameUI;
        private readonly GameStateMachine _stateMachine;

        public LoadProgressState(GameStateMachine stateMachine,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IGameUI gameUI)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _gameUI = gameUI;
        }

        public void Enter()
        {
            LoadProgress();

            _gameUI.OpenScreen(WindowID.Main);
            _stateMachine.Enter<PreGameLoopState>();
        }

        public void Exit()
        {
        }

        private void LoadProgress()
        {
            _progressService.DataProfiles = _saveLoadService.LoadAllProfiles();
        }
    }
}