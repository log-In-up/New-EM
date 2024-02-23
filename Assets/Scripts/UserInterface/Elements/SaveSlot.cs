using Assets.Scripts.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UserInterface.Elements
{
    [DisallowMultipleComponent]
    public class SaveSlot : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        private TextMeshProUGUI _saveSlotName;

        private string _slotName;

        public delegate void SlotName(string saveName);

        public event SlotName OnSelectSlotName;

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectSlotName?.Invoke(_slotName);
        }

        public void SetSlotData(SaveInfo saveInfo)
        {
            DateTime newDateTime = DateTime.FromBinary(saveInfo.LastUpdated);

            _slotName = saveInfo.Name;
            _saveSlotName.text = $"{saveInfo.Name} - {newDateTime}";
        }
    }
}