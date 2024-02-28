namespace Assets.Scripts.Infrastructure.Services.PauseAndContinue
{
    public interface IPauseContinueService : IService
    {
        bool IsPaused { get; }

        void Continue();

        void Pause();
    }
}