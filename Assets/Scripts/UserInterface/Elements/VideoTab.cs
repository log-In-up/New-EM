using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.Settings;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Elements
{
    public class VideoTab : SettingsTab
    {
        [SerializeField]
        private Toggle _bloom, _depthOfField, _fullscreen, _motionBlur, _vignette;

        [SerializeField]
        private Slider _fieldOfView, _fpsLimiter;

        [SerializeField]
        private Button _gamma;

        [SerializeField]
        private TMP_Dropdown _quality, _resolution, _motionBlurQuality;

        [SerializeField]
        private GameObject _resolutionParent, _motionBlurDropdownParent;

        private ICameraService _cameraService;
        private int _currentResolutionIndex, _motionBlurQualityIndex;
        private IGameDialogUI _gameDialogUI;
        private IGraphicsService _graphicsService;
        private Resolution[] _resolutions;
        private ISettingsService _settingsService;

        public override void SetSettings()
        {
            base.SetSettings();

            _graphicsService.ApplyResolutionData();
            _graphicsService.WriteData();
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _cameraService = serviceLocator.GetService<ICameraService>();
            _gameDialogUI = serviceLocator.GetService<IGameDialogUI>();
            _graphicsService = serviceLocator.GetService<IGraphicsService>();
            _settingsService = serviceLocator.GetService<ISettingsService>();

            SettingParametersFromService();
        }

        private void OnChangeBloom(bool value)
        {
            _graphicsService.SetBloom(value);
        }

        private void OnChangeDepthOfField(bool value)
        {
            _graphicsService.SetDepthOfField(value);
        }

        private void OnChangeFieldOfView(float value)
        {
            _cameraService.SetFieldOfView(value);
        }

        private void OnChangeFullscreen(bool value)
        {
            _graphicsService.SetResolutionData(_resolutions[_currentResolutionIndex].width, _resolutions[_currentResolutionIndex].height, value);
        }

        private void OnChangeMotionBlur(bool value)
        {
            _motionBlurDropdownParent.SetActive(value);

            _graphicsService.SetMoutionBlur(value, _motionBlurQualityIndex);
        }

        private void OnChangeMotionBlurQuality(int value)
        {
            _motionBlurQualityIndex = value;

            _graphicsService.SetMoutionBlur(_motionBlur.isOn, value);
        }

        private void OnChangeQualityLevel(int value)
        {
            _graphicsService.SetQualityLevel(value);
        }

        private void OnChangeResolution(int value)
        {
            _currentResolutionIndex = value;

            _graphicsService.SetResolutionData(_resolutions[_currentResolutionIndex].width, _resolutions[_currentResolutionIndex].height, _fullscreen.isOn);
        }

        private void OnChangeVignette(bool value)
        {
            _graphicsService.SetVignette(value);
        }

        private void OnClickGamma()
        {
            _gameDialogUI.OpenDialogWindow(DialogWindowID.GammaSetup);
        }

        private void OnDisable()
        {
            _resolutionParent.SetActive(true);
            _fullscreen.gameObject.SetActive(true);

            _bloom.onValueChanged.RemoveListener(OnChangeBloom);
            _depthOfField.onValueChanged.RemoveListener(OnChangeDepthOfField);
            _fieldOfView.onValueChanged.RemoveListener(OnChangeFieldOfView);
            _fpsLimiter.onValueChanged.RemoveListener(OnFPSValueChange);
            _fullscreen.onValueChanged.RemoveListener(OnChangeFullscreen);
            _gamma.onClick.RemoveListener(OnClickGamma);
            _motionBlur.onValueChanged.RemoveListener(OnChangeMotionBlur);
            _motionBlurQuality.onValueChanged.RemoveListener(OnChangeMotionBlurQuality);
            _quality.onValueChanged.RemoveListener(OnChangeQualityLevel);
            _resolution.onValueChanged.RemoveListener(OnChangeResolution);
            _vignette.onValueChanged.RemoveListener(OnChangeVignette);
        }

        private void OnEnable()
        {
            SettingParametersFromService();

            if (SystemInfo.deviceType != DeviceType.Desktop)
            {
                _resolutionParent.SetActive(false);
                _fullscreen.gameObject.SetActive(false);
            }

            _bloom.onValueChanged.AddListener(OnChangeBloom);
            _depthOfField.onValueChanged.AddListener(OnChangeDepthOfField);
            _fieldOfView.onValueChanged.AddListener(OnChangeFieldOfView);
            _fpsLimiter.onValueChanged.AddListener(OnFPSValueChange);
            _fullscreen.onValueChanged.AddListener(OnChangeFullscreen);
            _gamma.onClick.AddListener(OnClickGamma);
            _motionBlur.onValueChanged.AddListener(OnChangeMotionBlur);
            _motionBlurQuality.onValueChanged.AddListener(OnChangeMotionBlurQuality);
            _quality.onValueChanged.AddListener(OnChangeQualityLevel);
            _resolution.onValueChanged.AddListener(OnChangeResolution);
            _vignette.onValueChanged.AddListener(OnChangeVignette);
        }

        private void OnFPSValueChange(float value)
        {
            if (value.ApproximatelyEqual(_fpsLimiter.minValue, 0.001f))
            {
                _graphicsService.SetMaxFPS(-1);
            }
            else
            {
                _graphicsService.SetMaxFPS((int)value);
            }
        }

        private void SettingParametersFromService()
        {
            SetupQualityDropdown();

            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                SetupResolutionDropdown();

                _fullscreen.isOn = _settingsService.SettingsData.Fullscreen;
            }

            _bloom.isOn = _settingsService.SettingsData.Bloom;
            _depthOfField.isOn = _settingsService.SettingsData.DepthOfField;
            SetupMotionBlur();
            _vignette.isOn = _settingsService.SettingsData.Vignette;

            SetupFrameRate();
        }

        private void SetupFrameRate()
        {
            if (_settingsService.SettingsData.TargetFrameRate <= ((int)_fpsLimiter.minValue))
            {
                _fpsLimiter.value = _fpsLimiter.minValue;
            }
            else
            {
                _fpsLimiter.value = _settingsService.SettingsData.TargetFrameRate;
            }
        }

        private void SetupMotionBlur()
        {
            _motionBlur.isOn = _settingsService.SettingsData.MotionBlur;

            _motionBlurDropdownParent.SetActive(_motionBlur.isOn);

            List<string> qualityNames = new();
            foreach (MotionBlurQuality motionBlurQuality in (MotionBlurQuality[])Enum.GetValues(typeof(MotionBlurQuality)))
            {
                qualityNames.Add(motionBlurQuality.ToString());
            }

            _motionBlurQuality.ClearOptions();
            _motionBlurQuality.AddOptions(qualityNames);

            _motionBlurQuality.SetValueWithoutNotify(_settingsService.SettingsData.MotionBlurQuality);
        }

        private void SetupQualityDropdown()
        {
            List<string> qualityNames = new();
            foreach (string name in QualitySettings.names)
            {
                qualityNames.Add(name);
            }

            _quality.ClearOptions();
            _quality.AddOptions(qualityNames);

            int input = QualitySettings.GetQualityLevel();
            _quality.SetValueWithoutNotify(input);
        }

        private void SetupResolutionDropdown()
        {
            _resolutions = Screen.resolutions;

            List<string> resolutions = new();
            for (int index = 0; index < _resolutions.Length; index++)
            {
                resolutions.Add($"[{_resolutions[index].width} x {_resolutions[index].width}] - [{_resolutions[index].refreshRateRatio} Hz]");

                if (_resolutions[index].width == Screen.width && _resolutions[index].height == Screen.height)
                {
                    _currentResolutionIndex = index;
                }
            }

            _resolution.ClearOptions();
            _resolution.AddOptions(resolutions);

            _resolution.SetValueWithoutNotify(_currentResolutionIndex);
        }
    }
}