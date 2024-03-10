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

        public event ISaveSlot.SlotName OnSelectSlotName;

        public GameObject GameObject => gameObject;

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