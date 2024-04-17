using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Elements
{
    public class SoundsTab : SettingsTab
    {
        [SerializeField]
        private Slider _soundMain;

        private ISettingsService _settingsService;
        private IAudioService _audioService;

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _settingsService = serviceLocator.GetService<ISettingsService>();
            _audioService = serviceLocator.GetService<IAudioService>();

            GetSettings();
        }

        private void GetSettings()
        {
            _soundMain.value = _settingsService.SettingsData.MasterVolume;
        }

        public override void SetSettings()
        {
            base.SetSettings();

            _audioService.WriteData();
        }

        private void OnEnable()
        {
            _soundMain.onValueChanged.AddListener(OnChangeMainChannel);
        }

        private void OnChangeMainChannel(float value)
        {
            _audioService.SetMasterVolume(value);
        }

        private void OnDisable()
        {
            _soundMain.onValueChanged.RemoveListener(OnChangeMainChannel);
        }
    }
}