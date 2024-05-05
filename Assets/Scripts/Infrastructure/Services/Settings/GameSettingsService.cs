using Assets.Scripts.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public abstract class GameSettingsService<T> : ISettingsService where T : SettingsData
    {
        protected const string SETTINGS_FOLDER = "Settings";
        protected readonly string _dataDirPath, _dataFileName;
        private readonly IAudioService _audioService;
        private readonly ICameraService _cameraService;
        private readonly IGraphicsService _graphicsService;
        private T _settingsData;

        public GameSettingsService(
           IAudioService audioService,
           ICameraService cameraService,
           IGraphicsService graphicsService,
           string dataDirPath,
           string dataFileName)
        {
            _audioService = audioService;
            _cameraService = cameraService;
            _graphicsService = graphicsService;

            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
        }

        public bool DataAreEqual()
        {
            bool audio = _audioService.DataAreEqual();
            bool camera = _cameraService.DataAreEqual();
            bool graphics = _graphicsService.DataAreEqual();

            return audio && camera && graphics;
        }

        public DataType GetData<DataType>() where DataType : SettingsData
        {
            return _settingsData.GetType().Equals(typeof(DataType)) ? _settingsData as DataType : null;
        }

        public async Task Initialize<DataType>() where DataType : SettingsData
        {
            string settingsDirectory = Path.Combine(_dataDirPath, SETTINGS_FOLDER);
            if (!Directory.Exists(settingsDirectory))
            {
                try
                {
                    Directory.CreateDirectory(settingsDirectory);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to create sound settings directory.\n{exception}");
                }
            }
            _settingsData = await Load();

            await _audioService.Initialize(_settingsData);
            await _cameraService.Initialize(_settingsData);
            await _graphicsService.Initialize(_settingsData);
        }

        public void Invert()
        {
            _audioService.Invert();
            _cameraService.Invert();
            _graphicsService.Invert();
        }

        public Task Save()
        {
            _audioService.WriteData();
            _cameraService.WriteData();
            _graphicsService.WriteData();

            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SETTINGS_FOLDER, _dataFileName);
                try
                {
                    WriteDataToFile(fullPath, _settingsData);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to save data to file: {fullPath}.\n{exception}");
                }
            });
        }

        protected virtual Task<T> Load()
        {
            int qualityLevel = QualitySettings.GetQualityLevel();

            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SETTINGS_FOLDER, _dataFileName);
                T loadedData = null;

                if (File.Exists(fullPath))
                {
                    try
                    {
                        string dataToLoad = string.Empty;

                        using (FileStream stream = new(fullPath, FileMode.Open))
                        {
                            using StreamReader reader = new(stream);
                            dataToLoad = reader.ReadToEnd();
                        }

                        loadedData = JsonUtility.FromJson<T>(dataToLoad);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Error occured when trying to load data from file: {fullPath}.\n{exception}");
                    }
                }
                else
                {
                    loadedData = (T)Activator.CreateInstance(typeof(T));

                    loadedData.QualityLevel = qualityLevel;

                    WriteDataToFile(fullPath, loadedData);
                }

                return loadedData;
            });
        }

        protected void WriteDataToFile(string fullPath, T settingsData)
        {
            string dataToStore = JsonUtility.ToJson(settingsData, true);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(dataToStore);
        }
    }
}