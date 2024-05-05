using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Utility;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class AudioService : IAudioService
    {
        private const string MASTER_GROUP = "MasterGroup";
        private readonly IAssetProvider _assetProvider;
        private readonly AssetReference _audioMixerReference;
        private AudioMixer _audioMixer;
        private SettingsData _settingsData;

        public AudioService(IAssetProvider assetProvider,
           AssetReference audioMixerReference)
        {
            _assetProvider = assetProvider;
            _audioMixerReference = audioMixerReference;
        }

        ~AudioService()
        {
            _audioMixer = null;
        }

        public event IProcessSettings.SettingsChanged OnSettingsChanged;

        public bool DataAreEqual()
        {
            foreach ((float, float) item in GetFloatDataList())
            {
                if (item.Item1.ApproximatelyEqual(item.Item2, 0.001f))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task Initialize<SettingsType>(SettingsType settingsData) where SettingsType : SettingsData
        {
            _settingsData = settingsData;

            _audioMixer = await _assetProvider.LoadWithoutCleaning<AudioMixer>(_audioMixerReference);

            SetFloat(MASTER_GROUP, _settingsData.MasterVolume);
        }

        public void Invert()
        {
            SetFloat(MASTER_GROUP, _settingsData.MasterVolume);
        }

        public void SetMasterVolume(float value)
        {
            SetFloat(MASTER_GROUP, value);

            OnSettingsChanged?.Invoke();
        }

        public void WriteData()
        {
            _settingsData.MasterVolume = GetFloat(MASTER_GROUP);
        }

        private float GetFloat(string name)
        {
            if (_audioMixer.GetFloat(name, out float value))
            {
                return Mathf.Pow(10, (value / 20));
            }
            else
            {
                throw new NullReferenceException($"Audio mixer does not have an open parameter {name}");
            }
        }

        private (float, float)[] GetFloatDataList()
        {
            return new (float, float)[]
            {
                (GetFloat(MASTER_GROUP) , _settingsData.MasterVolume)
            };
        }

        private void SetFloat(string name, float value)
        {
            float volume = Mathf.Log10(value) * 20;
            _audioMixer.SetFloat(name, volume);
        }
    }
}