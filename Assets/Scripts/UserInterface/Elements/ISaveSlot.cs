using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.UserInterface.Elements
{
    public interface ISaveSlot
    {
        delegate void SlotName(string saveName);

        event SlotName OnSelectSlotName;

        GameObject GameObject { get; }

        void SetSlotData(SaveInfo saveInfo);
    }
}