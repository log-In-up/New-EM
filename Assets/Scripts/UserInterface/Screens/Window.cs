using UnityEngine;

namespace Assets.Scripts.UserInterface.Screens
{
    [DisallowMultipleComponent]
    public abstract class Window : MonoBehaviour
    {
        private GameUI _gameUi = null;
        private bool _isOpened = default;
        public GameUI GameUI => _gameUi;
        public abstract WindowID ID { get; }
        public bool IsOpen => _isOpened;

        public virtual void Activate()
        {
            _isOpened = true;
            gameObject.SetActive(_isOpened);
        }

        public virtual void Deactivate()
        {
            _isOpened = false;
            gameObject.SetActive(_isOpened);
        }

        public void SetScreenData(GameUI gameUi)
        {
            _gameUi = gameUi;
            _isOpened = gameObject.activeInHierarchy;
        }

        public virtual void Setup()
        { }
    }
}