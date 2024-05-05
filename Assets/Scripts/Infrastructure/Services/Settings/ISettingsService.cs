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
        /// Search for the game parameters file.
        /// </summary>
        /// <typeparam name="DataType">The file type we are trying to get.</typeparam>
        /// <returns>Game settings file for editing.</returns>
        DataType GetData<DataType>() where DataType : SettingsData;

        /// <summary>
        /// Compares data with saved data.
        /// </summary>
        /// <returns>Are the data equal?</returns>
        bool DataAreEqual();

        /// <summary>
        /// Initializes the settings system.
        /// </summary>
        /// <typeparam name="DataType">The file type with which the system is initialized.</typeparam>
        /// <returns>Async task.</returns>
        Task Initialize<DataType>() where DataType : SettingsData;

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