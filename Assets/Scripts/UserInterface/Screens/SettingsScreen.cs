using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class SettingsScreen : Window
    {
        [SerializeField]
        private Button _close;

        public override WindowID ID => WindowID.Settings;

        public override void Activate()
        {
            _close.onClick.AddListener(OnClickClose);

            base.Activate();
        }

        public override void Deactivate()
        {
            _close.onClick.RemoveListener(OnClickClose);

            base.Deactivate();
        }

        public override void Setup()
        {
            base.Setup();
        }

        private void OnClickClose()
        {
            GameUI.OpenScreen(WindowID.Main);
        }
    }
}