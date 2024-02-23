using Assets.Scripts.UserInterface.Screens;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UserInterface
{
    [DisallowMultipleComponent]
    public class GameUI : MonoBehaviour, IGameUI
    {
        [SerializeField]
        private List<Window> _windows;

        public void Initialize()
        {
            InitializeScreens();
        }

        public void OpenScreen(WindowID screenID)
        {
            foreach (Window window in _windows)
            {
                if (window.ID.Equals(screenID))
                {
                    window.Activate();
                }
                else if (window.IsOpen)
                {
                    window.Deactivate();
                }
            }
        }

        private void InitializeScreens()
        {
            foreach (Window screen in _windows)
            {
                screen.SetScreenData(this);

                screen.Setup();
            }
        }
    }
}