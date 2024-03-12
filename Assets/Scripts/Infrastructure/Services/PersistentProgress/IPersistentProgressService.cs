using Assets.Scripts.Data;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    /// <summary>
    /// The Persistent Progress Service is responsible for accessing save data.
    /// </summary>
    public interface IPersistentProgressService : IService
    {
        /// <summary>
        /// Current game data.
        /// </summary>
        GameData CurrentGameData { get; set; }

        /// <summary>
        /// All game data files.
        /// </summary>
        ObservableDictionary<string, GameData> ObservableDataSlots { get; set; }
    }
}