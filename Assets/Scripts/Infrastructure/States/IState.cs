namespace Assets.Scripts.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}