using Assets.Scripts.Data;
using System.Collections.Generic;

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
        void CreateNew(string slotId);

        /// <summary>
        /// Deletes a save.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        void Delete(string slotId);

        /// <summary>
        /// Loads save data.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Game save data.</returns>
        GameData Load(string slotId);

        /// <summary>
        /// Loads all saved slots into the game.
        /// </summary>
        /// <returns>All saved slots with links to them.</returns>
        Dictionary<string, GameData> LoadAllSlots();

        /// <summary>
        /// Loads the last saved slot.
        /// </summary>
        void LoadRecentlyUpdatedSave();

        /// <summary>
        /// Check for the existence of a saved slot.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        /// <returns>Does a saved slot exist?</returns>
        bool SlotExist(string slotId);

        /// <summary>
        /// Changes the slot name.
        /// </summary>
        /// <param name="dataProfile">Data profile to rename.</param>
        /// <param name="newSlotId">New slot name.</param>
        void Rename(KeyValuePair<string, GameData> dataProfile, string newSlotId);

        /// <summary>
        /// Saves the game slot.
        /// </summary>
        /// <param name="slotId">Slot name.</param>
        void Save(string slotId);
    }
}