using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Assets.Scripts.Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private InputActionsMap _inputActions;

        public InputService()
        {
            _inputActions = new InputActionsMap();
            _inputActions.Enable();

            _inputActions.UserInterface.Cancel.performed += OnCancel;
        }

        ~InputService()
        {
            _inputActions.UserInterface.Cancel.performed -= OnCancel;

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