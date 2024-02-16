using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.Infrastructure.States
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState>() where TState : class, IState;

        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;

        void InitializeStateMashine();
    }
}