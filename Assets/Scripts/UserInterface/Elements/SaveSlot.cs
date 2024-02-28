using Assets.Scripts.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UserInterface.Elements
{
    [DisallowMultipleComponent]
    public class SaveSlot : MonoBehaviour, ISelectHandler, ISaveSlot
    {
        [SerializeField]
        private TextMeshProUGUI _saveSlotName;

        private string _slotName;

        public GameObject GameObject => gameObject;

        public event ISaveSlot.SlotName OnSelectSlotName;

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