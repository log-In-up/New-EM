using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Assets.Scripts.Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private DefaultInputActions _inputActions;

        public InputService()
        {
            _inputActions = new DefaultInputActions();
            _inputActions.Enable();

            _inputActions.UI.Cancel.performed += OnCancel;
        }

        ~InputService()
        {
            _inputActions.UI.Cancel.performed -= OnCancel;

            _inputActions.Disable();
            _inputActions = null;
        }

        public event IInputService.CancelEventHandler OnClickCancel;

        private void OnCancel(CallbackContext context)
        {
            OnClickCancel?.Invoke();
        }
    }
}