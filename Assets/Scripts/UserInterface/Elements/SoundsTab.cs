using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Elements
{
    public class SoundsTab : SettingsTab
    {
        [SerializeField]
        private Slider _soundMain;

        private IAudioService _audioService;

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _audioService = serviceLocator.GetService<IAudioService>();

            GetSettings();
        }

        private void GetSettings()
        {
            _soundMain.value = _settingsData.MasterVolume;
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