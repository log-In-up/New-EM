namespace Assets.Scripts.Infrastructure.States
{
    /// <summary>
    /// The interface is responsible for entering a new state.
    /// </summary>
    public interface IState : IExitableState
    {
        /// <summary>
        /// Actions performed upon entering the current state.
        /// </summary>
        void Enter();
    }
}