using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class GraphicsService : IGraphicsService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly AssetReference _volumeProfileReference;
        private Bloom _bloom;
        private DepthOfField _depthOfField;
        private bool _fullscreen;
        private LiftGammaGain _liftGammaGain;
        private MotionBlur _motionBlur;
        private int _qualityLevel;
        private int _screenWidth, _screenHeight;
        private SettingsData _settingsData;
        private Vignette _vignette;
        private VolumeProfile _volumeProfile;

        public GraphicsService(IAssetProvider assetProvider,
            AssetReference volumeProfileReference)
        {
            _assetProvider = assetProvider;

            _volumeProfileReference = volumeProfileReference;
        }

        ~GraphicsService()
        {
            _bloom = null;
            _depthOfField = null;
            _liftGammaGain = null;
            _motionBlur = null;
            _vignette = null;
            _volumeProfile = null;
        }

        public event IProcessSettings.SettingsChanged OnSettingsChanged;

        public void ApplyResolutionData()
        {
            if (SystemInfo.deviceType != DeviceType.Desktop) return;

            if (ResolutionDataAreEqual()) return;

            _settingsData.ScreenWidth = _screenWidth;
            _settingsData.ScreenHeight = _screenHeight;
            _settingsData.Fullscreen = _fullscreen;

            Screen.SetResolution(_screenWidth, _screenHeight, _fullscreen);
        }

        public bool DataAreEqual()
        {
            foreach ((bool, bool) item in GetBoolDataList())
            {
                if (item.Item1 == item.Item2)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            foreach ((int, int) item in GetIntDataList())
            {
                if (item.Item1 == item.Item2)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            foreach ((float, float) item in GetFloatDataList())
            {
                if (item.Item1 == item.Item2)
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

        public async Task Initialize(SettingsData settingsData)
        {
            _settingsData = settingsData;

            _volumeProfile = await _assetProvider.LoadWithoutCleaning<VolumeProfile>(_volumeProfileReference);

            RetrievingComponentsFromVolumeProfile();
            SetData();
        }

        public void Invert()
        {
            SetData();
        }

        public void SetBloom(bool active)
        {
            _bloom.active = active;

            OnSettingsChanged?.Invoke();
        }

        public void SetDepthOfField(bool active)
        {
            _depthOfField.active = active;

            OnSettingsChanged?.Invoke();
        }

        public void SetGamma(float gamma)
        {
            _liftGammaGain.gamma.Override(new Vector4(1.0f, 1.0f, 1.0f, gamma));

            OnSettingsChanged?.Invoke();
        }

        public void SetMaxFPS(int maxFPS)
        {
            Application.targetFrameRate = maxFPS;

            OnSettingsChanged?.Invoke();
        }

        public void SetMoutionBlur(bool active, int motionBlurQuality)
        {
            _motionBlur.active = active;
            _motionBlur.quality.value = (MotionBlurQuality)motionBlurQuality;

            OnSettingsChanged?.Invoke();
        }

        public void SetQualityLevel(int qualityLevel)
        {
            _qualityLevel = qualityLevel;

            QualitySettings.SetQualityLevel(qualityLevel, false);

            OnSettingsChanged?.Invoke();
        }

        public void SetResolutionData(int width, int height, bool fullscreen)
        {
            _screenWidth = width;
            _screenHeight = height;
            _fullscreen = fullscreen;

            OnSettingsChanged?.Invoke();
        }

        public void SetVignette(bool active)
        {
            _vignette.active = active;

            OnSettingsChanged?.Invoke();
        }

        public void WriteData()
        {
            _settingsData.QualityLevel = QualitySettings.GetQualityLevel();

            _settingsData.Bloom = _bloom.active;

            _settingsData.DepthOfField = _depthOfField.active;

            _settingsData.MotionBlur = _motionBlur.active;
            _settingsData.MotionBlurQuality = (int)_motionBlur.quality.value;

            _settingsData.Vignette = _vignette.active;

            _settingsData.Gamma = _liftGammaGain.gamma.value.w;

            _settingsData.TargetFrameRate = Application.targetFrameRate;
        }

        private (bool, bool)[] GetBoolDataList()
        {
            return new (bool, bool)[]
            {
                (_bloom.active ,_settingsData.Bloom),
                (_depthOfField.active , _settingsData.DepthOfField),
                (_fullscreen ,_settingsData.Fullscreen),
                (_motionBlur.active ,_settingsData.MotionBlur),
                (_vignette.active , _settingsData.Vignette)
            };
        }

        private (float, float)[] GetFloatDataList()
        {
            return new (float, float)[]
            {
                (_liftGammaGain.gamma.value.w , _settingsData.Gamma)
            };
        }

        private (int, int)[] GetIntDataList()
        {
            return new (int, int)[]
            {
                ((int)_motionBlur.quality.value , _settingsData.MotionBlurQuality),
                (QualitySettings.GetQualityLevel() , _settingsData.QualityLevel),
                (_screenWidth , _settingsData.ScreenWidth),
                (_screenHeight , _settingsData.ScreenHeight),
                (Application.targetFrameRate, _settingsData.TargetFrameRate)
            };
        }

        private bool ResolutionDataAreEqual()
        {
            return _settingsData.ScreenWidth == _screenWidth && _settingsData.ScreenHeight == _screenHeight && _settingsData.Fullscreen == _fullscreen;
        }

        private C Retrieve<C>() where C : VolumeComponent
        {
            return _volumeProfile.TryGet(out C component) ? component : throw new NullReferenceException(nameof(component));
        }

        private void RetrievingComponentsFromVolumeProfile()
        {
            _bloom = Retrieve<Bloom>();
            _depthOfField = Retrieve<DepthOfField>();
            _motionBlur = Retrieve<MotionBlur>();
            _vignette = Retrieve<Vignette>();
            _liftGammaGain = Retrieve<LiftGammaGain>();
        }

        private void SetData()
        {
            _qualityLevel = _settingsData.QualityLevel;
            QualitySettings.SetQualityLevel(_qualityLevel, true);

            _screenWidth = _settingsData.ScreenWidth;
            _screenHeight = _settingsData.ScreenHeight;
            _fullscreen = _settingsData.Fullscreen;

            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                Screen.SetResolution(_screenWidth, _screenHeight, _fullscreen);
            }

            _bloom.active = _settingsData.Bloom;

            _depthOfField.active = _settingsData.DepthOfField;

            _motionBlur.active = _settingsData.MotionBlur;
            _motionBlur.quality.value = (MotionBlurQuality)_settingsData.MotionBlurQuality;

            _vignette.active = _settingsData.Vignette;

            _liftGammaGain.gamma.Override(new Vector4(1.0f, 1.0f, 1.0f, _settingsData.Gamma));
            Application.targetFrameRate = _settingsData.TargetFrameRate;
        }
    }
}