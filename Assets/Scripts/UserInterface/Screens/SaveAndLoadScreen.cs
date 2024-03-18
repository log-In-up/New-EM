using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UserInterface.Elements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class SaveAndLoadScreen : Screen
    {
        [SerializeField]
        private Button _changeName;

        [SerializeField]
        private Button _close;

        [SerializeField]
        private Button _createOverrideSave;

        [SerializeField]
        private Button _createSave;

        [SerializeField]
        private Button _deleteSave;

        [SerializeField]
        private Button _loadSave;

        [SerializeField]
        private SaveSlot _saveSlotExample;

        [SerializeField]
        private RectTransform _viewportParent;

        private IGameDialogUI _gameDialogUI;
        private IInputService _inputService;
        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;
        private string _saveName;
        private List<ISaveSlot> _saveSlots;
        private IGameStateMachine _stateMachine;

        public override ScreenID ID => ScreenID.SafeAndLoad;

        public override void Activate()
        {
            _changeName.onClick.AddListener(OnClickChangeSaveName);
            _close.onClick.AddListener(OnClickClose);

            if (GameUI.PeekScreen() == ScreenID.GamePauseScreen)
            {
                _createSave.gameObject.SetActive(true);
                _createSave.onClick.AddListener(OnClickCreateSave);

                _createOverrideSave.gameObject.SetActive(true);
                _createOverrideSave.onClick.AddListener(OnClickCreateOverrideSave);
            }
            else
            {
                _createOverrideSave.gameObject.SetActive(false);
                _createSave.gameObject.SetActive(false);
            }

            _deleteSave.onClick.AddListener(OnClickDeleteSave);
            _loadSave.onClick.AddListener(OnClickLoadSave);

            _inputService.OnClickCancel += OnClickClose;

            _saveName = string.Empty;
            SetButtonInteraction(false);

            CreateSaveSlots();

            base.Activate();
        }

        public override void Deactivate()
        {
            _changeName.onClick.RemoveListener(OnClickChangeSaveName);
            _close.onClick.RemoveListener(OnClickClose);
            _createOverrideSave.onClick.RemoveListener(OnClickCreateOverrideSave);
            _createSave.onClick.RemoveListener(OnClickCreateSave);
            _deleteSave.onClick.RemoveListener(OnClickDeleteSave);
            _loadSave.onClick.RemoveListener(OnClickLoadSave);

            _inputService.OnClickCancel -= OnClickClose;

            base.Deactivate();

            _createOverrideSave.gameObject.SetActive(true);
            _createSave.gameObject.SetActive(true);

            ClearSaveSlots();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            _saveSlots = new List<ISaveSlot>();

            base.Setup(serviceLocator);

            _inputService = serviceLocator.GetService<IInputService>();
            _persistentProgressService = serviceLocator.GetService<IPersistentProgressService>();
            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
            _stateMachine = serviceLocator.GetService<IGameStateMachine>();
            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();

            _gameDialogUI.AddWindowActions(DialogWindowID.ChangeSave, SaveCollectionChanged, SaveCollectionChanged);
            _gameDialogUI.AddWindowActions(DialogWindowID.OverwriteSave, SaveCollectionChanged, SaveCollectionChanged);
        }

        private void ClearSaveSlots()
        {
            foreach (ISaveSlot item in _saveSlots)
            {
                item.OnSelectSlotName -= OnSelectSlotName;
                Destroy(item.GameObject);
            }

            _saveSlots.Clear();
        }

        private void CreateSaveSlots()
        {
            foreach (KeyValuePair<string, GameData> item in _persistentProgressService.ObservableDataSlots)
            {
                ISaveSlot saveSlot = Instantiate(_saveSlotExample, _viewportParent);
                saveSlot.SetSlotData(item.Value.SaveInfo);

                _saveSlots.Add(saveSlot);
            }

            foreach (ISaveSlot item in _saveSlots)
            {
                item.OnSelectSlotName += OnSelectSlotName;
            }
        }

        private void OnClickChangeSaveName()
        {
            _gameDialogUI.OpenDialogWindow(DialogWindowID.ChangeSave, _saveName);
        }

        private void OnClickClose()
        {
            GameUI.OpenScreen(GameUI.PopScreen());
        }

        private void OnClickCreateOverrideSave()
        {
            _gameDialogUI.OpenDialogWindow(DialogWindowID.OverwriteSave, _saveName);
        }

        private void OnClickCreateSave()
        {
            _gameDialogUI.OpenDialogWindow(DialogWindowID.CreateSave);
        }

        private async void OnClickDeleteSave()
        {
            if (string.IsNullOrEmpty(_saveName)) return;

            await _saveLoadService.Delete(_saveName);

            SaveCollectionChanged();

            _saveName = string.Empty;
            SetButtonInteraction(false);
        }

        private void SetButtonInteraction(bool value)
        {
            _createOverrideSave.interactable = value;
            _changeName.interactable = value;
            _deleteSave.interactable = value;
            _loadSave.interactable = value;
        }

        private async void OnClickLoadSave()
        {
            if (string.IsNullOrEmpty(_saveName)) return;

            _persistentProgressService.CurrentGameData = await _saveLoadService.Load(_saveName);

            _stateMachine.Enter<LoadLevelState, string>(_persistentProgressService.CurrentGameData.CurrentLevel);
        }

        private void OnSelectSlotName(string saveName)
        {
            _saveName = saveName;

            SetButtonInteraction(true);
        }

        private void SaveCollectionChanged()
        {
            ClearSaveSlots();
            CreateSaveSlots();
        }
    }
}