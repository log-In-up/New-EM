using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.Settings;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.UserInterface.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class SettingsScreen : Screen
    {
        [SerializeField]
        private Button _applySettings;

        [SerializeField]
        private Button _close;

        [SerializeField]
        private VideoTab _videoTab;

        [SerializeField]
        private SoundsTab _soundTab;

        private IAudioService _audioService;
        private ICameraService _cameraService;
        private IGameDialogUI _gameDialogUI;
        private IGraphicsService _graphicsService;
        private IInputService _inputService;
        private ISettingsService _settingsService;
        public override ScreenID ID => ScreenID.Settings;

        public override void Activate()
        {
            _applySettings.onClick.AddListener(OnClickApply);
            _close.onClick.AddListener(OnClickClose);

            _audioService.OnSettingsChanged += SettingsChanged;
            _graphicsService.OnSettingsChanged += SettingsChanged;
            _cameraService.OnSettingsChanged += SettingsChanged;

            _inputService.OnClickCancel += OnClickClose;

            _applySettings.interactable = !_settingsService.DataAreEqual();

            base.Activate();
        }

        public override void Deactivate()
        {
            _applySettings.onClick.RemoveListener(OnClickApply);
            _close.onClick.RemoveListener(OnClickClose);

            _audioService.OnSettingsChanged -= SettingsChanged;
            _graphicsService.OnSettingsChanged -= SettingsChanged;
            _cameraService.OnSettingsChanged -= SettingsChanged;

            _inputService.OnClickCancel -= OnClickClose;

            base.Deactivate();

            _applySettings.interactable = false;
        }

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _audioService = serviceLocator.GetService<IAudioService>();
            _cameraService = serviceLocator.GetService<ICameraService>();
            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();
            _graphicsService = serviceLocator.GetService<IGraphicsService>();
            _inputService = serviceLocator.GetService<IInputService>();
            _settingsService = serviceLocator.GetService<ISettingsService>();

            _videoTab.Setup(serviceLocator);
            _soundTab.Setup(serviceLocator);
        }

        private void OnClickApply()
        {
            _applySettings.interactable = false;

            _videoTab.SetSettings();
            _soundTab.SetSettings();

            _settingsService.Save();
        }

        private void OnClickClose()
        {
            if (_gameDialogUI.IsActive) return;

            if (_settingsService.DataAreEqual())
            {
                GameUI.OpenScreen(GameUI.PopScreen());
            }
            else
            {
                _gameDialogUI.OpenDialogWindow(DialogWindowID.ApplySettings);
            }
        }

        private void SettingsChanged()
        {
            _applySettings.interactable = !_settingsService.DataAreEqual();
        }
    }
}