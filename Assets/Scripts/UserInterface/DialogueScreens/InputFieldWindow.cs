using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public abstract class InputFieldWindow : DialogueWindow
    {
        [SerializeField]
        protected TMP_InputField _inputField;

        protected string _inputFieldData;
        protected ISaveLoadService _saveLoadService;

        public override void Activate()
        {
            _inputField.onValueChanged.AddListener(OnChangeInputField);

            _inputFieldData = "";
            _inputField.text = _inputFieldData;
            _save.interactable = false;

            base.Activate();
        }

        public override void Deactivate()
        {
            _inputField.onValueChanged.AddListener(OnChangeInputField);

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
        }

        protected virtual void OnChangeInputField(string value)
        {
        }
    }
}