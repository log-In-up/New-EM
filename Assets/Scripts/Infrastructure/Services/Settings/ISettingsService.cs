using Assets.Scripts.Data;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    /// <summary>
    /// The settings service is responsible for interacting with settings for graphics, sounds, etc.
    /// </summary>
    public interface ISettingsService : IService
    {
        /// <summary>
        /// Settings data
        /// </summary>
        SettingsData SettingsData { get; }

        /// <summary>
        /// Initializes the settings system.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Initialize();

        /// <summary>
        /// Saves the settings data.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Save();

        /// <summary>
        /// Sets the value of the main sound.
        /// </summary>
        /// <param name="value">Value to set.</param>
        void SetMasterVolume(float value);
    }
}