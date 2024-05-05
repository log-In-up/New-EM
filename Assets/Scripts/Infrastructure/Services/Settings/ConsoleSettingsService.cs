using Assets.Scripts.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class ConsoleSettingsService : GameSettingsService<ConsoleSettingsData>
    {
        public ConsoleSettingsService(
            IAudioService audioService,
            ICameraService cameraService,
            IGraphicsService graphicsService,
            string dataDirPath,
            string dataFileName) : base(
                audioService,
                cameraService,
                graphicsService,
                dataDirPath,
                dataFileName)
        {
        }

        protected override Task<ConsoleSettingsData> Load()
        {
            int qualityLevel = QualitySettings.GetQualityLevel();

            return Task.Run(() =>
            {
                string fullPath = Path.Combine(_dataDirPath, SETTINGS_FOLDER, _dataFileName);
                ConsoleSettingsData loadedData = null;

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

                        loadedData = JsonUtility.FromJson<ConsoleSettingsData>(dataToLoad);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Error occured when trying to load data from file: {fullPath}.\n{exception}");
                    }
                }
                else
                {
                    loadedData = new ConsoleSettingsData
                    {
                        QualityLevel = qualityLevel
                    };

                    WriteDataToFile(fullPath, loadedData);
                }

                return loadedData;
            });
        }
    }
}