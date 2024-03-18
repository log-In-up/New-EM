namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class CreateSaveWindow : InputFieldWindow
    {
        public override DialogWindowID ID => DialogWindowID.CreateSave;

        protected override async void OnChangeInputField(string value)
        {
            _save.interactable = false;

            if (await _saveLoadService.SlotExist(value))
            {
                _save.interactable = false;
            }
            else
            {
                _save.interactable = !string.IsNullOrEmpty(value);
            }

            _inputFieldData = value;
        }

        protected override async void OnClickPositive()
        {
            await _saveLoadService.CreateNew(_inputFieldData);

            base.OnClickPositive();
        }
    }
}