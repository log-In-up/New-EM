namespace Assets.Scripts.Infrastructure.Services.Settings
{
    /// <summary>
    /// The audio service is responsible for interacting with sound settings.
    /// </summary>
    public interface IAudioService : IProcessSettings
    {
        /// <summary>
        /// Sets the value of the main sound.
        /// </summary>
        /// <param name="value">Value to set.</param>
        void SetMasterVolume(float value);
    }
}