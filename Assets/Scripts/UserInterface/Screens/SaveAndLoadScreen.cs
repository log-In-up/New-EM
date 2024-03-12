using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UserInterface.Elements;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

namespace Assets.Scripts.UserInterface.Screens
{
    public class SaveAndLoadScreen : Screen
    {
        [SerializeField]
        private Button _changeName;

        [SerializeField]
        private Button _close;

        [SerializeField]
        private Button _createSave;

        [SerializeField]
        private Button _deleteSave;

        private IGameDialogUI _gameDialogUI;

        private IInputService _inputService;

        [SerializeField]
        private Button _loadSave;

        private IPersistentProgressService _persistentProgressService;

        private ISaveLoadService _saveLoadService;

        private string _saveName;

        [SerializeField]
        private SaveSlot _saveSlotExample;

        private List<ISaveSlot> _saveSlots;

        private IGameStateMachine _stateMachine;

        [SerializeField]
        private RectTransform _viewportParent;

        public override ScreenID ID => ScreenID.SafeAndLoad;

        public override void Activate()
        {
            if (GameUI.PeekScreen() == ScreenID.GamePauseScreen)
            {
                _createSave.gameObject.SetActive(true);
                _createSave.onClick.AddListener(OnClickCreateSave);
            }
            else
            {
                _createSave.gameObject.SetActive(false);
            }

            _changeName.onClick.AddListener(OnClickChangeSaveName);
            _close.onClick.AddListener(OnClickClose);
            _deleteSave.onClick.AddListener(OnClickDeleteSave);
            _loadSave.onClick.AddListener(OnClickLoadSave);

            _inputService.OnClickCancel += OnClickClose;
            _persistentProgressService.ObservableDataSlots.CollectionChanged += SaveCollectionChanged;

            _deleteSave.interactable = false;
            _loadSave.interactable = false;
            _changeName.interactable = false;
            _saveName = string.Empty;

            CreateSaveSlots();

            base.Activate();
        }

        public override void Deactivate()
        {
            _changeName.onClick.RemoveListener(OnClickChangeSaveName);
            _close.onClick.RemoveListener(OnClickClose);
            _createSave.onClick.RemoveListener(OnClickCreateSave);
            _deleteSave.onClick.RemoveListener(OnClickDeleteSave);
            _loadSave.onClick.RemoveListener(OnClickLoadSave);

            _inputService.OnClickCancel -= OnClickClose;
            _persistentProgressService.ObservableDataSlots.CollectionChanged -= SaveCollectionChanged;

            base.Deactivate();

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

        private void OnClickCreateSave()
        {
            _gameDialogUI.OpenDialogWindow(DialogWindowID.CreateSave);
        }

        private void OnClickDeleteSave()
        {
            if (string.IsNullOrEmpty(_saveName)) return;

            _saveLoadService.Delete(_saveName);

            _saveName = string.Empty;

            _changeName.interactable = false;
            _deleteSave.interactable = false;
            _loadSave.interactable = false;
        }

        private void OnClickLoadSave()
        {
            if (string.IsNullOrEmpty(_saveName)) return;

            _persistentProgressService.CurrentGameData = _saveLoadService.Load(_saveName);

            _stateMachine.Enter<LoadLevelState, string>(_persistentProgressService.CurrentGameData.CurrentLevel);
        }

        private void OnSelectSlotName(string saveName)
        {
            _saveName = saveName;

            _changeName.interactable = true;
            _deleteSave.interactable = true;
            _loadSave.interactable = true;
        }

        private void SaveCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            ClearSaveSlots();
            CreateSaveSlots();
        }
    }
}