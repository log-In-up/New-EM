using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.Settings;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.UserInterface.Elements;
using UnityEngine;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class ApplySettingsWindow : WarningWindow
    {
        [SerializeField]
        private VideoTab _videoTab;

        [SerializeField]
        private SoundsTab _soundTab;

        private IGameUI _gameUI;
        private ISettingsService _settingsService;

        public override DialogWindowID ID => DialogWindowID.ApplySettings;

        public override void Activate()
        {
            _apply.interactable = true;

            base.Activate();
        }

        public override void Deactivate()
        {
            _apply.interactable = true;

            base.Deactivate();
        }

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _gameUI = serviceLocator.GetService<IGameUI>();
            _settingsService = serviceLocator.GetService<ISettingsService>();
        }

        protected override void OnClickNegative()
        {
            _settingsService.Invert();

            base.OnClickNegative();
            _gameUI.OpenScreen(_gameUI.PopScreen());
        }

        protected override async void OnClickPositive()
        {
            _apply.interactable = false;

            _videoTab.SetSettings();
            _soundTab.SetSettings();

            await _settingsService.Save();

            base.OnClickPositive();
            _gameUI.OpenScreen(_gameUI.PopScreen());
        }
    }
}