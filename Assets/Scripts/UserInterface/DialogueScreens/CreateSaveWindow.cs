namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class CreateSaveWindow : InputFieldWindow
    {
        public override DialogWindowID ID => DialogWindowID.CreateSave;

        protected override void OnChangeInputField(string value)
        {
            if (_saveLoadService.SlotExist(value))
            {
                _save.interactable = false;
            }
            else
            {
                _save.interactable = !string.IsNullOrEmpty(value);
            }

            _inputFieldData = value;
        }

        protected override void OnClickSave()
        {
            _saveLoadService.CreateNew(_inputFieldData);

            base.OnClickSave();
        }
    }
}