using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using UnityEngine;

namespace Assets.Scripts.UserInterface.Screens
{
    [DisallowMultipleComponent]
    public abstract class Screen : MonoBehaviour
    {
        private IGameUI _gameUi = null;
        private bool _isOpened = default;
        public IGameUI GameUI => _gameUi;
        public abstract ScreenID ID { get; }
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

        public void SetScreenData(IGameUI gameUi)
        {
            _gameUi = gameUi;
            _isOpened = gameObject.activeInHierarchy;
        }

        public virtual void Setup(ServiceLocator serviceLocator)
        { }

        public virtual void SendPayload<TPayload>(TPayload payload) where TPayload : class
        { }
    }
}