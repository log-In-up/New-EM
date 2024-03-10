namespace Assets.Scripts.Infrastructure.States
{
    /// <summary>
    /// The interface is responsible for exiting the current state.
    /// </summary>
    public interface IExitableState
    {
        /// <summary>
        /// Actions performed before exiting the current state.
        /// </summary>
        void Exit();
    }
}