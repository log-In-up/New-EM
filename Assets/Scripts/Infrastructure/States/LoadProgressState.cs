using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.UserInterface;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameUI _gameUI;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;

        public LoadProgressState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IGameUI gameUI)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _gameUI = gameUI;
        }

        public void Enter()
        {
            LoadProgress();

            _sceneLoader.LoadScreensaverScene(OnSceneLoad);
        }

        public void Exit()
        {
        }

        private void LoadProgress()
        {
            _progressService.DataProfiles = _saveLoadService.LoadAllProfiles();
        }

        private void OnSceneLoad()
        {
            _gameUI.OpenScreen(WindowID.Main);
            _stateMachine.Enter<PreGameLoopState>();
        }
    }
}