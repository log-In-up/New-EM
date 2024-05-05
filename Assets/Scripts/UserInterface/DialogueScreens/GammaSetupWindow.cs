using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class GammaSetupWindow : DialogueWindow
    {
        [SerializeField]
        private Slider _gammaSlider;

        private float _gammaInitial;
        private IGraphicsService _graphicsService;
        private ISettingsService _settingsService;

#if UNITY_ANDROID || UNITY_IOS
        protected HandheldSettingsData _settingsData;
#elif UNITY_PS3 || UNITY_PS4 || UNITY_SAMSUNGTV || UNITY_XBOX360 || UNITY_XBOXONE
        protected ConsoleSettingsData _settingsData;
#elif UNITY_STANDALONE
        protected DesktopSettingsData _settingsData;
#endif

        public override DialogWindowID ID => DialogWindowID.GammaSetup;

        public override void Activate()
        {
            _gammaSlider.onValueChanged.AddListener(OnChangeGamma);

            _gammaInitial = _settingsData.Gamma;
            _gammaSlider.value = _gammaInitial;

            base.Activate();
        }

        public override void Deactivate()
        {
            _gammaSlider.onValueChanged.RemoveListener(OnChangeGamma);

            base.Deactivate();
        }

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _graphicsService = serviceLocator.GetService<IGraphicsService>();
            _settingsService = serviceLocator.GetService<ISettingsService>();

#if UNITY_ANDROID || UNITY_IOS
            _settingsData = _settingsService.GetData<HandheldSettingsData>();
#elif UNITY_PS3 || UNITY_PS4 || UNITY_SAMSUNGTV || UNITY_XBOX360 || UNITY_XBOXONE
            _settingsData = _settingsService.GetData<ConsoleSettingsData>();
#elif UNITY_STANDALONE
            _settingsData = _settingsService.GetData<DesktopSettingsData>();
#endif

            _gammaSlider.value = _settingsData.Gamma;
        }

        protected override void OnClickNegative()
        {
            base.OnClickNegative();

            _graphicsService.SetGamma(_gammaInitial);
        }

        private void OnChangeGamma(float value)
        {
            _graphicsService.SetGamma(value);
        }
    }
}