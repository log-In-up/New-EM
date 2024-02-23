using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UserInterface.Elements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class SaveAndLoadScreen : Window
    {
        [SerializeField]
        private Button _close;

        [SerializeField]
        private Button _deleteSave;

        [SerializeField]
        private Button _loadSave;

        private IPersistentProgressService _persistentProgressService;

        private ISaveLoadService _saveLoadService;

        private string _saveName;

        [SerializeField]
        private SaveSlot _saveSlotExample;

        private List<SaveSlot> _saveSlots;

        private IGameStateMachine _stateMachine;

        [SerializeField]
        private RectTransform _viewportParent;

        public override WindowID ID => WindowID.SafeAndLoad;

        public override void Activate()
        {
            _close.onClick.AddListener(OnClickClose);
            _deleteSave.onClick.AddListener(OnClickDeleteSave);
            _loadSave.onClick.AddListener(OnClickLoadSave);

            _deleteSave.interactable = false;
            _loadSave.interactable = false;
            _saveName = string.Empty;

            CreateSaveSlots();

            base.Activate();
        }

        public override void Deactivate()
        {
            _close.onClick.RemoveListener(OnClickClose);
            _deleteSave.onClick.RemoveListener(OnClickDeleteSave);
            _loadSave.onClick.RemoveListener(OnClickLoadSave);

            base.Deactivate();

            ClearSaveSlots();
        }

        public override void Setup()
        {
            _saveSlots = new List<SaveSlot>();

            base.Setup();

            ServiceLocator serviceLocator = ServiceLocator.Container;

            _persistentProgressService = serviceLocator.GetService<IPersistentProgressService>();
            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
            _stateMachine = serviceLocator.GetService<IGameStateMachine>();
        }

        private void ClearSaveSlots()
        {
            foreach (SaveSlot item in _saveSlots)
            {
                item.OnSelectSlotName -= OnSelectSlotName;
                Destroy(item.gameObject);
            }

            _saveSlots.Clear();
        }

        private void CreateSaveSlots()
        {
            foreach (KeyValuePair<string, GameData> item in _persistentProgressService.DataProfiles)
            {
                SaveSlot saveSlot = Instantiate(_saveSlotExample, _viewportParent);
                saveSlot.SetSlotData(item.Value.SaveInfo);

                _saveSlots.Add(saveSlot);
            }

            foreach (SaveSlot item in _saveSlots)
            {
                item.OnSelectSlotName += OnSelectSlotName;
            }
        }

        private void OnClickClose()
        {
            GameUI.OpenScreen(WindowID.Main);
        }

        private void OnClickDeleteSave()
        {
            if (string.IsNullOrEmpty(_saveName)) return;

            ClearSaveSlots();

            _saveLoadService.Delete(_saveName);
            string fullPath = _saveLoadService.GetPath(_saveName);

            _persistentProgressService.DataProfiles.Remove(fullPath);

            CreateSaveSlots();

            _saveName = string.Empty;
            _deleteSave.interactable = false;
            _loadSave.interactable = false;
        }

        private void OnClickLoadSave()
        {
            if (string.IsNullOrEmpty(_saveName)) return;

            _persistentProgressService.GameData = _saveLoadService.Load(_saveName);

            _stateMachine.Enter<LoadLevelState, string>(_persistentProgressService.GameData.CurrentLevel);
        }

        private void OnSelectSlotName(string saveName)
        {
            _saveName = saveName;

            _deleteSave.interactable = true;
            _loadSave.interactable = true;
        }
    }
}