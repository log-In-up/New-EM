using Assets.Scripts.Infrastructure;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.PauseAndContinue;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class GamePauseScreen : Screen
    {
        [SerializeField]
        private Button _continue;

        [SerializeField]
        private Button _load;

        [SerializeField]
        private Button _quit;

        [SerializeField]
        private Button _settings;

        private IInputService _inputService;
        private IPauseContinueService _pauseContinueService;
        private ISceneLoader _sceneLoaderService;
        private IGameStateMachine _stateMachine;

        public override ScreenID ID => ScreenID.GamePauseScreen;

        public override void Activate()
        {
            _inputService.OnClickCancel += OnClickCancel;
            _continue.onClick.AddListener(OnClickCancel);
            _load.onClick.AddListener(OnClickLoad);
            _quit.onClick.AddListener(OnClickQuit);
            _settings.onClick.AddListener(OnClickSettings);

            _quit.interactable = true;

            base.Activate();
        }

        public override void Deactivate()
        {
            _inputService.OnClickCancel -= OnClickCancel;
            _continue.onClick.RemoveListener(OnClickCancel);
            _load.onClick.RemoveListener(OnClickLoad);
            _quit.onClick.RemoveListener(OnClickQuit);
            _settings.onClick.RemoveListener(OnClickSettings);

            _quit.interactable = true;

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _inputService = serviceLocator.GetService<IInputService>();
            _pauseContinueService = serviceLocator.GetService<IPauseContinueService>();
            _sceneLoaderService = serviceLocator.GetService<ISceneLoader>();
            _stateMachine = serviceLocator.GetService<IGameStateMachine>();
        }

        private void OnClickCancel()
        {
            _pauseContinueService.Continue();
            GameUI.OpenScreen(ScreenID.Gameplay);
        }

        private void OnClickLoad()
        {
            GameUI.PushScreen(ID);
            GameUI.OpenScreen(ScreenID.SafeAndLoad);
        }

        private void OnClickQuit()
        {
            _quit.interactable = false;

            _sceneLoaderService.LoadScreensaverScene(OnLoadMain);
        }

        private void OnLoadMain()
        {
            _pauseContinueService.Continue();

            GameUI.OpenScreen(ScreenID.Main);
            _stateMachine.Enter<PreGameLoopState>();
        }

        private void OnClickSettings()
        {
            GameUI.PushScreen(ID);
            GameUI.OpenScreen(ScreenID.Settings);
        }
    }
}