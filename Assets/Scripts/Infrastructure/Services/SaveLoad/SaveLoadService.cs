using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
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

        public void CreateNew(string slotId)
        {
            GameData gameData = new GameData();

            _persistentProgressService.CurrentGameData = gameData;
            Save(slotId);

            string fullPath = Path.Combine(_dataDirPath, slotId, _dataFileName);
            _persistentProgressService.ObservableDataProfiles.Add(fullPath, gameData);
        }

        public void Delete(string slotId)
        {
            if (string.IsNullOrEmpty(slotId)) return;

            string fullPath = Path.Combine(_dataDirPath, slotId, _dataFileName);

            try
            {
                if (File.Exists(fullPath))
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                    _persistentProgressService.ObservableDataProfiles.Remove(fullPath);
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
        }

        public GameData Load(string slotId)
        {
            string fullPath = Path.Combine(_dataDirPath, slotId, _dataFileName);
            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (_useIncription)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to load data from file: {fullPath}.\n{exception}");
                }
            }

            return loadedData;
        }

        public Dictionary<string, GameData> LoadAllSlots()
        {
            Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

            IEnumerable<DirectoryInfo> directoryInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                string slotId = directoryInfo.FullName;
                string fullPath = Path.Combine(_dataDirPath, slotId, _dataFileName);

                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"Skipping directory when loading all profiles because it does not contain data: {slotId}.");
                    continue;
                }

                GameData data = Load(slotId);

                if (data != null)
                {
                    profileDictionary.Add(slotId, data);
                }
                else
                {
                    Debug.LogError($"Tried to load profile but something went wrong. slotId: {slotId}.");
                }
            }

            return profileDictionary;
        }

        public void LoadRecentlyUpdatedSave()
        {
            string slotId = GetMostRecentlyUpdatedslotId();

            _persistentProgressService.CurrentGameData = Load(slotId);
        }

        public bool SlotExist(string slotId)
        {
            string fullPath = Path.Combine(_dataDirPath, slotId, _dataFileName);
            string directoryPath = Path.GetDirectoryName(fullPath);

            bool result = _persistentProgressService.ObservableDataProfiles.ContainsKey(fullPath) ||
                _persistentProgressService.ObservableDataProfiles.ContainsKey(directoryPath);

            return result;
        }

        public void Rename(KeyValuePair<string, GameData> dataProfile, string newSlotId)
        {
            string currentDirectoryName = dataProfile.Key;
            string newDirectoryName = Path.Combine(_dataDirPath, newSlotId);

            try
            {
                Directory.Move(currentDirectoryName, newDirectoryName);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error occured when trying to rename file.\n{exception}");
            }

            GameData data = _persistentProgressService.ObservableDataProfiles[currentDirectoryName];
            _persistentProgressService.ObservableDataProfiles.Remove(currentDirectoryName);

            DirectoryInfo newDirectory = new DirectoryInfo(newDirectoryName);
            _persistentProgressService.ObservableDataProfiles.Add(newDirectory.FullName, data);

            dataProfile.Value.SaveInfo.Name = newSlotId;
        }

        public void Save(string slotId)
        {
            InformProgressWriters();

            string fullPath = Path.Combine(_dataDirPath, slotId, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                SaveInfo saveInfo = new SaveInfo(DateTime.Now.Ticks, slotId);
                _persistentProgressService.CurrentGameData.SaveInfo = saveInfo;

                string dataToStore = JsonUtility.ToJson(_persistentProgressService.CurrentGameData, true);

                if (_useIncription)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error occured when trying to save data to file: {fullPath}.\n{exception}");
            }
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

        private string GetMostRecentlyUpdatedslotId()
        {
            string mostRecentSlotId = string.Empty;

            Dictionary<string, GameData> profilesGameData = LoadAllSlots();

            foreach (KeyValuePair<string, GameData> pair in profilesGameData)
            {
                string slotId = pair.Key;
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
        }

        private void InformProgressWriters()
        {
            foreach (ISaveProgress progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.UpdateProgress(_persistentProgressService.CurrentGameData);
            }
        }
    }
}