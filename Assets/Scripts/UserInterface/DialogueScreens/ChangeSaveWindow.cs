using System;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class ChangeSaveWindow : InputFieldWindow
    {
        private string _slotToRename;
        public override DialogWindowID ID => DialogWindowID.ChangeSave;

        public override void SendPayload<TPayload>(TPayload payload)
        {
            _slotToRename = payload as string;
        }

        protected override void OnChangeInputField(string value)
        {
            _save.interactable = !string.IsNullOrEmpty(value);
            _inputFieldData = value;
        }

        protected override async void OnClickPositive()
        {
            if (await _saveLoadService.SlotExist(_slotToRename))
            {
                await _saveLoadService.Rename(_slotToRename, _inputFieldData);
            }

            base.OnClickPositive();
        }
    }
}