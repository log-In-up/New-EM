using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class GammaSetupWindow : DialogueWindow
    {
        [SerializeField]
        private Slider _gammaSlider;

        private IGraphicsService _graphicsService;
        private ISettingsService _settingsService;
        private float _gammaInitial;

        public override DialogWindowID ID => DialogWindowID.GammaSetup;

        public override void Activate()
        {
            _gammaSlider.onValueChanged.AddListener(OnChangeGamma);

            _gammaInitial = _settingsService.SettingsData.Gamma;
            _gammaSlider.value = _gammaInitial;

            base.Activate();
        }

        public override void Deactivate()
        {
            _gammaSlider.onValueChanged.RemoveListener(OnChangeGamma);

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _graphicsService = serviceLocator.GetService<IGraphicsService>();
            _settingsService = serviceLocator.GetService<ISettingsService>();

            _gammaSlider.value = _settingsService.SettingsData.Gamma;
        }

        private void OnChangeGamma(float value)
        {
            _graphicsService.SetGamma(value);
        }

        protected override void OnClickNegative()
        {
            base.OnClickNegative();

            _graphicsService.SetGamma(_gammaInitial);
        }
    }
}