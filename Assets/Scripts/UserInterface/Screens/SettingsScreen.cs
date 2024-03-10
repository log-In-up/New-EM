using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class SettingsScreen : Screen
    {
        [SerializeField]
        private Button _close;

        private IInputService _inputService;

        public override ScreenID ID => ScreenID.Settings;

        public override void Activate()
        {
            _close.onClick.AddListener(OnClickClose);
            _inputService.OnClickCancel += OnClickClose;

            base.Activate();
        }

        public override void Deactivate()
        {
            _close.onClick.RemoveListener(OnClickClose);
            _inputService.OnClickCancel -= OnClickClose;

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _inputService = serviceLocator.GetService<IInputService>();
        }

        private void OnClickClose()
        {
            GameUI.OpenScreen(GameUI.PopScreen());
        }
    }
}