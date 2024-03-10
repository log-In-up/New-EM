namespace Assets.Scripts.Infrastructure.Services.Input
{
    /// <summary>
    /// The service responsible for tracking input.
    /// </summary>
    public interface IInputService : IService
    {
        /// <summary>
        /// Cancel button handler.
        /// </summary>
        delegate void CancelEventHandler();

        /// <summary>
        /// Cancel button pressed.
        /// </summary>
        event CancelEventHandler OnClickCancel;
    }
}