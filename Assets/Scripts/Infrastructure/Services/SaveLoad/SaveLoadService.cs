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

        public SaveLoadService(IGameFactory gameFactory, IPersistentProgressService persistentProgressService, string dataDirPath, string dataFileName)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;

            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;

            _useIncription = false;
            _ecriptionCodeWord = "";
        }

        public SaveLoadService(IGameFactory gameFactory, IPersistentProgressService persistentProgressService, string dataDirPath, string dataFileName, string ecriptionCodeWord)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;

            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;

            _useIncription = true;
            _ecriptionCodeWord = ecriptionCodeWord;
        }

        public void CreateNewGame()
        {
            _persistentProgressService.GameData = new GameData();
            Save("New Game");
        }

        public void Delete(string profileId)
        {
            if (string.IsNullOrEmpty(profileId)) return;

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            try
            {
                if (File.Exists(fullPath))
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else
                {
                    Debug.LogWarning($"Tried to delete profile data, but data was not found at path: {fullPath}");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to delete profile data for profileId: {profileId} at path: {fullPath}.\n{exception}");
            }
        }

        public string GetMostRecentlyUpdatedProfileId()
        {
            string mostRecentProfileId = null;

            Dictionary<string, GameData> profilesGameData = LoadAllProfiles();

            foreach (KeyValuePair<string, GameData> pair in profilesGameData)
            {
                string profileId = pair.Key;
                GameData gameData = pair.Value;

                if (gameData == null) continue;

                if (mostRecentProfileId == null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    long dateData = profilesGameData[mostRecentProfileId].SaveInfo.LastUpdated;
                    DateTime mostRecentDateTime = DateTime.FromBinary(dateData);
                    DateTime newDateTime = DateTime.FromBinary(gameData.SaveInfo.LastUpdated);

                    if (newDateTime > mostRecentDateTime)
                    {
                        mostRecentProfileId = profileId;
                    }
                }
            }
            return mostRecentProfileId;
        }

        public string GetPath(string profileId)
        {
            return Path.GetDirectoryName(Path.Combine(_dataDirPath, profileId, _dataFileName));
        }

        public GameData Load(string profileId)
        {
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
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

        public Dictionary<string, GameData> LoadAllProfiles()
        {
            Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

            IEnumerable<DirectoryInfo> directoryInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                string profileId = directoryInfo.FullName;
                string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);

                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"Skipping directory when loading all profiles because it does not contain data: {profileId}.");
                    continue;
                }

                GameData data = Load(profileId);

                if (data != null)
                {
                    profileDictionary.Add(profileId, data);
                }
                else
                {
                    Debug.LogError($"Tried to load profile but something went wrong. ProfileID: {profileId}.");
                }
            }

            return profileDictionary;
        }

        public void LoadRecentlyUpdatedSave()
        {
            string profileId = GetMostRecentlyUpdatedProfileId();
            _persistentProgressService.GameData = Load(profileId);
        }

        public void Save(string profileId)
        {
            InformProgressWriters();

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                SaveInfo saveInfo = new SaveInfo(DateTime.Now.Ticks, profileId);
                _persistentProgressService.GameData.SaveInfo = saveInfo;

                string dataToStore = JsonUtility.ToJson(_persistentProgressService.GameData, true);

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
            string modifiedData = "";

            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _ecriptionCodeWord[i % _ecriptionCodeWord.Length]);
            }

            return modifiedData;
        }

        private void InformProgressWriters()
        {
            foreach (ISaveProgress progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.UpdateProgress(_persistentProgressService.GameData);
            }
        }
    }
}