using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string SAVES_FOLDER = "Saves";
        private readonly string _dataDirPath, _dataFileName, _ecriptionCodeWord;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly bool _useIncription;

        public SaveLoadService(IGameFactory gameFactory,
            IPersistentProgressService persistentProgressService,
            string dataDirPath,
            string dataFileName,
            string ecriptionCodeWord)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;

            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;

            _useIncription = !string.IsNullOrEmpty(ecriptionCodeWord);
            _ecriptionCodeWord = ecriptionCodeWord;
        }

        public Task CreateNew(string slotId)
        {
            return Task.Run(async () =>
            {
                GameData gameData = new GameData();

                _persistentProgressService.CurrentGameData = gameData;
                await Save(slotId);

                string fullPath = Path.Combine(_dataDirPath, SAVES_FOLDER, slotId, _dataFileName);
                _persistentProgressService.ObservableDataSlots.Add(fullPath, gameData);
            });
        }

        public Task Delete(string slotId)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(slotId)) return;

                string fullPath = Path.Combine(_dataDirPath, SAVES_FOLDER, slotId, _dataFileName);

                try
                {
                    if (File.Exists(fullPath))
                    {
                        Directory.Delete(Path.GetDirectoryName(fullPath), true);
                        _persistentProgressService.ObservableDataSlots.Remove(fullPath);
                    }
                    else
                    {
                        Debug.LogError($"Tried to delete profile data, but data was not found at path: {fullPath}");
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to delete profile data for slotId: {slotId} at path: {fullPath}.\n{exception}");
                }
            });
        }

        public Task Initialize()
        {
            return Task.Run(() =>
            {
                string savesDirectory = Path.Combine(_dataDirPath, SAVES_FOLDER);

                if (!Directory.Exists(savesDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(savesDirectory);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Error occured when trying to create save directory.\n{exception}");
                    }
                }
            });
        }

        public Task<GameData> Load(string slotId)
        {
            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SAVES_FOLDER, slotId, _dataFileName);
                GameData loadedData = null;

                if (File.Exists(fullPath))
                {
                    try
                    {
                        string dataToLoad = string.Empty;

                        using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                        {
                            using StreamReader reader = new StreamReader(stream);
                            dataToLoad = reader.ReadToEnd();
                        }

                        dataToLoad = ApplyIncription(dataToLoad);

                        loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Error occured when trying to load data from file: {fullPath}.\n{exception}");
                    }
                }

                return loadedData;
            });
        }

        public Task<Dictionary<string, GameData>> LoadAllSlots()
        {
            return Task.Run(async () =>
            {
                Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

                string savesDirectory = Path.Combine(_dataDirPath, SAVES_FOLDER);
                IEnumerable<DirectoryInfo> directoryInfos = new DirectoryInfo(savesDirectory).EnumerateDirectories();

                foreach (DirectoryInfo directoryInfo in directoryInfos)
                {
                    string slotId = directoryInfo.FullName;
                    string fullPath = Path.Combine(_dataDirPath, SAVES_FOLDER, directoryInfo.Name, _dataFileName);

                    if (!File.Exists(fullPath))
                    {
                        Debug.LogWarning($"Skipping directory when loading all profiles because it does not contain data: {slotId}.");
                        continue;
                    }

                    GameData data = await Load(directoryInfo.Name);

                    if (data != null)
                    {
                        profileDictionary.Add(fullPath, data);
                    }
                    else
                    {
                        Debug.LogError($"Tried to load profile but something went wrong. slotId: {slotId}.");
                    }
                }

                return profileDictionary;
            });
        }

        public Task LoadRecentlyUpdatedSave()
        {
            return Task.Run(async () =>
            {
                string slotId = await GetMostRecentlyUpdatedSlotId();

                _persistentProgressService.CurrentGameData = await Load(slotId);
            });
        }

        public Task Rename(string oldSlotId, string newSlotId)
        {
            return Task.Run(() =>
            {
                string oldDirectoryName = Path.Combine(_dataDirPath, SAVES_FOLDER, oldSlotId);
                string newDirectoryName = Path.Combine(_dataDirPath, SAVES_FOLDER, newSlotId);

                try
                {
                    Directory.Move(oldDirectoryName, newDirectoryName);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to rename file.\n{exception}");
                }

                oldDirectoryName = Path.Combine(_dataDirPath, SAVES_FOLDER, oldSlotId, _dataFileName);
                newDirectoryName = Path.Combine(_dataDirPath, SAVES_FOLDER, newSlotId, _dataFileName);

                GameData data = _persistentProgressService.ObservableDataSlots[oldDirectoryName];

                SaveInfo saveInfo = new SaveInfo(DateTime.Now.Ticks, newSlotId);
                data.SaveInfo = saveInfo;

                if (_persistentProgressService.CurrentGameData != null || _persistentProgressService.CurrentGameData == data)
                {
                    _persistentProgressService.CurrentGameData.SaveInfo = saveInfo;
                }

                string dataToStore = JsonUtility.ToJson(data, true);
                dataToStore = ApplyIncription(dataToStore);

                WriteDataToFile(newDirectoryName, dataToStore);

                _persistentProgressService.ObservableDataSlots.Remove(oldDirectoryName);
                _persistentProgressService.ObservableDataSlots.Add(newDirectoryName, data);
            });
        }

        public Task Save(string slotId)
        {
            return Task.Run(() =>
            {
                InformProgressWriters();

                string fullPath = Path.Combine(_dataDirPath, SAVES_FOLDER, slotId, _dataFileName);
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    SaveInfo saveInfo = new SaveInfo(DateTime.Now.Ticks, slotId);
                    _persistentProgressService.CurrentGameData.SaveInfo = saveInfo;

                    string dataToStore = JsonUtility.ToJson(_persistentProgressService.CurrentGameData, true);
                    dataToStore = ApplyIncription(dataToStore);

                    WriteDataToFile(fullPath, dataToStore);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to save data to file: {fullPath}.\n{exception}");
                }
            });
        }

        public Task<bool> SlotExist(string slotId)
        {
            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SAVES_FOLDER, slotId, _dataFileName);
                return _persistentProgressService.ObservableDataSlots.ContainsKey(fullPath);
            });
        }

        private string ApplyIncription(string dataToIncription)
        {
            if (_useIncription)
            {
                dataToIncription = EncryptDecrypt(dataToIncription);
            }

            return dataToIncription;
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = string.Empty;

            for (int index = 0; index < data.Length; index++)
            {
                modifiedData += (char)(data[index] ^ _ecriptionCodeWord[index % _ecriptionCodeWord.Length]);
            }

            return modifiedData;
        }

        private Task<string> GetMostRecentlyUpdatedSlotId()
        {
            return Task.Run(async () =>
            {
                string mostRecentSlotId = string.Empty;

                Dictionary<string, GameData> profilesGameData = await LoadAllSlots();

                foreach (KeyValuePair<string, GameData> pair in profilesGameData)
                {
                    string slotId = Path.GetFileName(Path.GetDirectoryName(pair.Key));
                    GameData gameData = pair.Value;

                    if (gameData == null) continue;

                    if (string.IsNullOrEmpty(mostRecentSlotId))
                    {
                        mostRecentSlotId = slotId;
                    }
                    else
                    {
                        long dateData = profilesGameData[mostRecentSlotId].SaveInfo.LastUpdated;
                        DateTime mostRecentDateTime = DateTime.FromBinary(dateData);
                        DateTime newDateTime = DateTime.FromBinary(gameData.SaveInfo.LastUpdated);

                        if (newDateTime > mostRecentDateTime)
                        {
                            mostRecentSlotId = slotId;
                        }
                    }
                }
                return mostRecentSlotId;
            });
        }

        private void InformProgressWriters()
        {
            foreach (ISaveProgress progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.UpdateProgress(_persistentProgressService.CurrentGameData);
            }
        }

        private void WriteDataToFile(string fullPath, string dataToStore)
        {
            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(dataToStore);
        }
    }
}