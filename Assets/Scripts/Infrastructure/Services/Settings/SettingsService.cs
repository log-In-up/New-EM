using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string MASTER_GROUP = "MasterGroup";
        private const string SETTINGS_FOLDER = "Settings";
        private readonly IAssetProvider _assetProvider;
        private readonly AssetReference _audioMixerReference;
        private readonly string _dataDirPath, _dataFileName;
        private AudioMixer _audioMixer;
        private SettingsData _settingsData;

        public SettingsService(IAssetProvider assetProvider,
            AssetReference audioMixerReference,
            string dataDirPath,
            string dataFileName)
        {
            _assetProvider = assetProvider;
            _audioMixerReference = audioMixerReference;

            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
        }

        ~SettingsService()
        {
            _audioMixer = null;
            _settingsData = null;
        }

        public SettingsData SettingsData => _settingsData;

        public async Task Initialize()
        {
            _audioMixer = await _assetProvider.Load<AudioMixer>(_audioMixerReference);

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

            SetSettingsFromDownloadedData();
        }

        public Task Save()
        {
            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SETTINGS_FOLDER, _dataFileName);
                try
                {
                    WriteData(fullPath, _settingsData);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to save data to file: {fullPath}.\n{exception}");
                }
            });
        }

        public void SetMasterVolume(float value)
        {
            _audioMixer.SetFloat(MASTER_GROUP, Mathf.Log10(value) * 20);
        }

        private Task<SettingsData> Load()
        {
            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SETTINGS_FOLDER, _dataFileName);
                SettingsData loadedData = null;

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

                        loadedData = JsonUtility.FromJson<SettingsData>(dataToLoad);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Error occured when trying to load data from file: {fullPath}.\n{exception}");
                    }
                }
                else
                {
                    loadedData = new SettingsData();

                    WriteData(fullPath, loadedData);
                }

                return loadedData;
            });
        }

        private void SetSettingsFromDownloadedData()
        {
            SetMasterVolume(_settingsData.MasterVolume);
        }

        private void WriteData(string fullPath, SettingsData settingsData)
        {
            string dataToStore = JsonUtility.ToJson(settingsData, true);

            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(dataToStore);
        }
    }
}