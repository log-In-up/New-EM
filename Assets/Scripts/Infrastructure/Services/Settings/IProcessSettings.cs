using Assets.Scripts.Data;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    /// <summary>
    /// Responsible for interacting with settings data.
    /// </summary>
    public interface IProcessSettings : IService
    {
        /// <summary>
        /// A delegate that handles settings changes.
        /// </summary>
        delegate void SettingsChanged();

        /// <summary>
        /// This event occurs when any of the settings are changed.
        /// </summary>
        event SettingsChanged OnSettingsChanged;

        /// <summary>
        /// Compares data with saved data.
        /// </summary>
        /// <returns>Are the data equal?</returns>
        bool DataAreEqual();

        /// <summary>
        /// Initializing the settings service.
        /// </summary>
        /// <typeparam name="SettingsType">Settings file type.</typeparam>
        /// <param name="settingsData">Settings data.</param>
        /// <returns>Async task.</returns>
        Task Initialize<SettingsType>(SettingsType settingsData) where SettingsType : SettingsData;

        /// <summary>
        /// Inverts settings to saved values.
        /// </summary>
        void Invert();

        /// <summary>
        /// Writes data to a file for saving.
        /// </summary>
        void WriteData();
    }
}