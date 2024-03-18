using Assets.Scripts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    /// <summary>
    /// The interface is responsible for processing in-game save game data.
    /// </summary>
    public interface ISaveLoadService : IService
    {
        /// <summary>
        /// Creates a new save.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Async task.</returns>
        Task CreateNew(string slotId);

        /// <summary>
        /// Deletes a save.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Async task.</returns>
        Task Delete(string slotId);

        /// <summary>
        /// Initializes the save system.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Initialize();

        /// <summary>
        /// Loads save data.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Game save data.</returns>
        /// <returns>Async task.</returns>
        Task<GameData> Load(string slotId);

        /// <summary>
        /// Loads all saved slots into the game.
        /// </summary>
        /// <returns>All saved slots with links to them.</returns>
        /// <returns>Async task.</returns>
        Task<Dictionary<string, GameData>> LoadAllSlots();

        /// <summary>
        /// Loads the last saved slot.
        /// </summary>
        /// <returns>Async task.</returns>
        Task LoadRecentlyUpdatedSave();

        /// <summary>
        /// Changes the slot name.
        /// </summary>
        /// <param name="oldSlotId">Data slot to rename.</param>
        /// <param name="newSlotId">New slot name.</param>
        /// <returns>Async task.</returns>
        Task Rename(string oldSlotId, string newSlotId);

        /// <summary>
        /// Saves the game slot.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Async task.</returns>
        Task Save(string slotId);

        /// <summary>
        /// Check for the existence of a saved slot.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Does a saved slot exist?</returns>
        /// <returns>Async task.</returns>
        Task<bool> SlotExist(string slotId);
    }
}