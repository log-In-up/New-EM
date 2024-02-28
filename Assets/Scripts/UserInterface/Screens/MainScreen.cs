using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID || UNITY_IOS
using Assets.Scripts.Infrastructure.Services.Input;
#endif

namespace Assets.Scripts.UserInterface.Screens
{
    public class MainScreen : Window
    {
        [SerializeField]
        private Button _continue;

        [SerializeField]
        private Button _load;

        [SerializeField]
        private Button _quit;

        [SerializeField]
        private Button _settings;

        [SerializeField]
        private Button _start;

        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;
        private IGameStateMachine _stateMachine;
#if UNITY_ANDROID || UNITY_IOS
        private IInputService _inputService;
#endif

        public override WindowID ID => WindowID.Main;

        public override void Activate()
        {
            GameUI.ClearScreens();
            GameUI.PushScreen(ID);

            if (_persistentProgressService.DataProfiles.Count <= 0)
            {
                _continue.gameObject.SetActive(false);
                _start.onClick.AddListener(OnClickStart);

                _load.interactable = false;
            }
            else
            {
                _start.gameObject.SetActive(false);
                _continue.onClick.AddListener(OnClickContinue);

                _load.interactable = true;
            }

            _load.onClick.AddListener(OnClickLoad);
            _quit.onClick.AddListener(OnClickQuit);
#if UNITY_ANDROID || UNITY_IOS
            _inputService.OnClickCancel += OnClickQuit;
#endif
            _settings.onClick.AddListener(OnClickSettings);

            base.Activate();
        }

        public override void Deactivate()
        {
            if (_persistentProgressService.DataProfiles.Count <= 0)
            {
                _continue.gameObject.SetActive(true);
                _start.onClick.RemoveListener(OnClickStart);
            }
            else
            {
                _start.gameObject.SetActive(true);
                _continue.onClick.RemoveListener(OnClickContinue);
            }

            _load.onClick.RemoveListener(OnClickLoad);
            _quit.onClick.RemoveListener(OnClickQuit);
#if UNITY_ANDROID || UNITY_IOS
            _inputService.OnClickCancel -= OnClickQuit;
#endif
            _settings.onClick.RemoveListener(OnClickSettings);

            _load.interactable = true;

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _persistentProgressService = serviceLocator.GetService<IPersistentProgressService>();
            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
            _stateMachine = serviceLocator.GetService<IGameStateMachine>();
#if UNITY_ANDROID || UNITY_IOS
            _inputService = serviceLocator.GetService<IInputService>();
#endif
        }

        private void OnClickContinue()
        {
            _saveLoadService.LoadRecentlyUpdatedSave();

            _stateMachine.Enter<LoadLevelState, string>(_persistentProgressService.GameData.CurrentLevel);
        }

        private void OnClickLoad()
        {
            GameUI.OpenScreen(WindowID.SafeAndLoad);
        }

        private void OnClickQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnClickSettings()
        {
            GameUI.OpenScreen(WindowID.Settings);
        }

        private void OnClickStart()
        {
            _saveLoadService.CreateNewGame();

            _stateMachine.Enter<LoadLevelState, string>(_persistentProgressService.GameData.CurrentLevel);
        }
    }
}