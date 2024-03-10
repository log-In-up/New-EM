using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.UserInterface.Elements
{
    /// <summary>
    /// Responsible for displaying data in the slot.
    /// </summary>
    public interface ISaveSlot
    {
        /// <summary>
        /// Handler for a slot when it is selected.
        /// </summary>
        /// <param name="saveName">Save slot name.</param>
        delegate void SlotName(string saveName);

        /// <summary>
        /// Event called when a slot is selected.
        /// </summary>
        event SlotName OnSelectSlotName;

        /// <summary>
        /// A GameObject that represents the save slot.
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// Displays the save data in the slot.
        /// </summary>
        /// <param name="saveInfo">Data to display.</param>
        void SetSlotData(SaveInfo saveInfo);
    }
}