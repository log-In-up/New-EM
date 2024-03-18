using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    [DisallowMultipleComponent]
    public abstract class DialogueWindow : MonoBehaviour
    {
        [SerializeField]
        protected Button _cancel;

        [SerializeField]
        protected Button _save;

        private IGameDialogUI _gameDialogUI;
        private bool _isOpened = false;
        private Action _onApply, _onCancel;
        public abstract DialogWindowID ID { get; }

        public bool IsOpen => _isOpened;

        public virtual void Activate()
        {
            _cancel.onClick.AddListener(OnClickNegative);
            _save.onClick.AddListener(OnClickPositive);

            _isOpened = true;
            gameObject.SetActive(_isOpened);
        }

        public void AddActions(Action onApply, Action onCancel)
        {
            _onApply = onApply;
            _onCancel = onCancel;
        }

        public virtual void Deactivate()
        {
            _cancel.onClick.RemoveListener(OnClickNegative);
            _save.onClick.RemoveListener(OnClickPositive);

            _isOpened = false;
            gameObject.SetActive(_isOpened);
        }

        public virtual void SendPayload<TPayload>(TPayload payload) where TPayload : class
        {
        }

        public virtual void Setup(ServiceLocator serviceLocator)
        {
            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();
        }

        protected virtual void OnClickNegative()
        {
            _onCancel?.Invoke();

            _gameDialogUI.CloseDialogWindows();
        }

        protected virtual void OnClickPositive()
        {
            _onApply?.Invoke();

            _gameDialogUI.CloseDialogWindows();
        }
    }
}