using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.Infrastructure.States
{
    /// <summary>
    /// The interface responsible for managing the game state machine.
    /// </summary>
    public interface IGameStateMachine : IService
    {
        /// <summary>
        /// Entering another state of the state machine.
        /// </summary>
        /// <typeparam name="TState">State type.</typeparam>
        void Enter<TState>() where TState : class, IState;

        /// <summary>
        /// Entering another state of the state machine with a payload.
        /// </summary>
        /// <typeparam name="TState">State type.</typeparam>
        /// <typeparam name="TPayload">Payload</typeparam>
        /// <param name="payload"></param>
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;

        /// <summary>
        /// Initializing the state machine.
        /// </summary>
        void InitializeStateMashine();
    }
}