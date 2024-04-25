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
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameStateMachine _stateMachine;

        public LoadProgressState(IGameStateMachine stateMachine,
            IGameUI gameUI,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _gameUI = gameUI;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
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