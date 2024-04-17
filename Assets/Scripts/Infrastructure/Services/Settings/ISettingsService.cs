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
        /// Settings data.
        /// </summary>
        SettingsData SettingsData { get; }

        /// <summary>
        /// Compares data with saved data.
        /// </summary>
        /// <returns>Are the data equal?</returns>
        bool DataAreEqual();

        /// <summary>
        /// Initializes the settings system.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Initialize();

        /// <summary>
        /// Inverts settings to saved values.
        /// </summary>
        void Invert();

        /// <summary>
        /// Saves the settings data.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Save();
    }
}