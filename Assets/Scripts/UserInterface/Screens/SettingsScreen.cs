using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.Settings;
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
        private Slider _soundMain;

        private IInputService _inputService;
        private ISettingsService _settingsService;

        public override ScreenID ID => ScreenID.Settings;

        public override void Activate()
        {
            _applySettings.onClick.AddListener(OnClickApply);
            _close.onClick.AddListener(OnClickClose);
            _soundMain.onValueChanged.AddListener(OnChangeMainChannel);

            _inputService.OnClickCancel += OnClickClose;

            SetSettings();

            _applySettings.interactable = false;

            base.Activate();
        }

        public override void Deactivate()
        {
            _applySettings.onClick.RemoveListener(OnClickApply);
            _close.onClick.RemoveListener(OnClickClose);
            _soundMain.onValueChanged.RemoveListener(OnChangeMainChannel);

            _inputService.OnClickCancel -= OnClickClose;

            base.Deactivate();

            _applySettings.interactable = false;
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _inputService = serviceLocator.GetService<IInputService>();
            _settingsService = serviceLocator.GetService<ISettingsService>();
        }

        private void CheckApplyInteraction()
        {
            if (_applySettings.interactable == false)
            {
                _applySettings.interactable = true;
            }
        }

        private void OnChangeMainChannel(float value)
        {
            CheckApplyInteraction();

            _settingsService.SetMasterVolume(value);
        }

        private void OnClickApply()
        {
            _applySettings.interactable = false;

            _settingsService.SettingsData.MasterVolume = _soundMain.value;

            _settingsService.Save();
        }

        private void OnClickClose()
        {
            GameUI.OpenScreen(GameUI.PopScreen());
        }

        private void SetSettings()
        {
            _soundMain.value = _settingsService.SettingsData.MasterVolume;
        }
    }
}