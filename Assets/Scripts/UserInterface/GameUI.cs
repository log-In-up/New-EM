using Assets.Scripts.Infrastructure.Services;
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

        private Stack<WindowID> _windowIDs;

        private void Awake()
        {
            _windowIDs = new Stack<WindowID>();
        }

        public void Initialize(ServiceLocator serviceLocator)
        {
            InitializeScreens(serviceLocator);
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

        private void InitializeScreens(ServiceLocator serviceLocator)
        {
            foreach (Window screen in _windows)
            {
                screen.SetScreenData(this);

                screen.Setup(serviceLocator);
            }
        }

        public void ClearScreens()
        {
            _windowIDs.Clear();
        }

        public WindowID PeekScreen()
        {
            return _windowIDs.Peek();
        }

        public void PushScreen(WindowID screenID)
        {
            _windowIDs.Push(screenID);
        }
    }
}