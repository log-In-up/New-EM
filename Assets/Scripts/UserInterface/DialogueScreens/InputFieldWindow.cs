using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public abstract class InputFieldWindow : DialogueWindow
    {
        [SerializeField]
        protected Button _cancel;

        [SerializeField]
        protected TMP_InputField _inputField;

        [SerializeField]
        protected Button _save;

        private IGameDialogUI _gameDialogUI;
        protected string _inputFieldData;
        protected ISaveLoadService _saveLoadService;

        public override void Activate()
        {
            _cancel.onClick.AddListener(OnClickCancel);
            _save.onClick.AddListener(OnClickSave);
            _inputField.onValueChanged.AddListener(OnChangeInputField);

            _inputFieldData = "";
            _inputField.text = _inputFieldData;
            _save.interactable = false;

            base.Activate();
        }

        public override void Deactivate()
        {
            _cancel.onClick.RemoveListener(OnClickCancel);
            _save.onClick.RemoveListener(OnClickSave);
            _inputField.onValueChanged.AddListener(OnChangeInputField);

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();
            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
        }

        protected virtual void OnChangeInputField(string value)
        {
        }

        protected virtual void OnClickCancel()
        {
            _gameDialogUI.CloseDialogWindows();
        }

        protected virtual void OnClickSave()
        {
            _gameDialogUI.CloseDialogWindows();
        }
    }
}