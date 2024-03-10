namespace Assets.Scripts.Infrastructure.States
{
    /// <summary>
    /// The interface is responsible for entering a new state.
    /// </summary>
    /// <typeparam name="TPayload">Type of payload.</typeparam>
    public interface IPayloadedState<TPayload> : IExitableState
    {
        /// <summary>
        /// Actions performed upon entering the current state.
        /// </summary>
        /// <param name="payload">Payload at entry.</param>
        void Enter(TPayload payload);
    }
}