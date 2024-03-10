namespace Assets.Scripts.Infrastructure.Services.PauseAndContinue
{
    /// <summary>
    /// Service for pausing the game.
    /// </summary>
    public interface IPauseContinueService : IService
    {
        /// <summary>
        /// Is the game paused?
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Unpauses the game.
        /// </summary>
        void Continue();

        /// <summary>
        /// Pauses the game.
        /// </summary>
        void Pause();
    }
}