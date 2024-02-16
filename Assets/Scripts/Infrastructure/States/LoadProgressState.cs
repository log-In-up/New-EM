using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameStateMachine _stateMachine;

        public LoadProgressState(GameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _stateMachine.Enter<LoadLevelState, string>(_progressService.GameData.CurrentLevel);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.DataProfiles = _saveLoadService.LoadAllProfiles();

            if (_progressService.DataProfiles.Count <= 0)
            {
                _progressService.GameData = new GameData();
                _saveLoadService.Save("New Game");
            }
            else
            {
                _progressService.GameData = _saveLoadService.Load("New Game");
            }
        }
    }
}