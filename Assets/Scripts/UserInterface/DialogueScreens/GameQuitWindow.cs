using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using UnityEngine;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class GameQuitWindow : DialogueWindow
    {
        public override DialogWindowID ID => DialogWindowID.GameQuit;

        private IGameDialogUI _gameDialogUI;

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();
        }

        protected override void OnClickNegative()
        {
            _gameDialogUI.CloseDialogWindows();
        }

        protected override void OnClickPositive()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}