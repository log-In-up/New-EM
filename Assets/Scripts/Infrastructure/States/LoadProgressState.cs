using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.UserInterface;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async void Enter()
        {
            await LoadProgress();

            _sceneLoader.LoadScreensaverScene(OnSceneLoad);
        }

        public void Exit()
        {
        }

        private async Task LoadProgress()
        {
            Dictionary<string, GameData> saveSlots = await _saveLoadService.LoadAllSlots();

            foreach (KeyValuePair<string, GameData> slot in saveSlots)
            {
                _progressService.ObservableDataSlots.Add(slot.Key, slot.Value);
            }
        }

        private void OnSceneLoad()
        {
            _gameUI.OpenScreen(ScreenID.Main);
            _stateMachine.Enter<PreGameLoopState>();
        }
    }
}