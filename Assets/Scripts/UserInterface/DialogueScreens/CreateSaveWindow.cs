using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class CreateSaveWindow : DialogueWindow
    {
        [SerializeField]
        private Button _cancel;

        [SerializeField]
        private Button _save;

        [SerializeField]
        private TMP_InputField _saveNameField;

        private IGameDialogUI _gameDialogUI;
        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;
        private string _saveName;

        public override DialogWindowID ID => DialogWindowID.CreateSave;

        public override void Activate()
        {
            _cancel.onClick.AddListener(OnClickCancel);
            _save.onClick.AddListener(OnClickSave);
            _saveNameField.onValueChanged.AddListener(OnChangeSaveNameInputField);

            base.Activate();

            _saveName = "";
            _saveNameField.text = _saveName;
            _save.interactable = false;
        }

        public override void Deactivate()
        {
            _cancel.onClick.RemoveListener(OnClickCancel);
            _save.onClick.RemoveListener(OnClickSave);
            _saveNameField.onValueChanged.AddListener(OnChangeSaveNameInputField);

            base.Deactivate();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();
            _persistentProgressService = serviceLocator.GetService<IPersistentProgressService>();
            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
        }

        private void OnChangeSaveNameInputField(string value)
        {
            if (_saveLoadService.SlotExist(value))
            {
                _save.interactable = false;
            }
            else
            {
                _save.interactable = !string.IsNullOrEmpty(value);
            }

            _saveName = value;
        }

        private void OnClickCancel()
        {
            _gameDialogUI.CloseDialogWindows();
        }

        private void OnClickSave()
        {
            _saveLoadService.CreateNew(_saveName);
            _gameDialogUI.CloseDialogWindows();
        }
    }
}