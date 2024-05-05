using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class DesktopGraphics : GraphicsService<DesktopSettingsData>
    {
        private bool _fullscreen;
        private int _screenWidth, _screenHeight;

        public DesktopGraphics(
            IAssetProvider assetProvider,
            AssetReference volumeProfileReference) : base(
                  assetProvider,
                  volumeProfileReference)
        {
        }

        public override void ApplyResolutionData()
        {
            if (ResolutionDataAreEqual()) return;

            _settingsData.Width = _screenWidth;
            _settingsData.Height = _screenHeight;
            _settingsData.Fullscreen = _fullscreen;

            Screen.SetResolution(_screenWidth, _screenHeight, _fullscreen);
        }

        private bool ResolutionDataAreEqual()
        {
            return _settingsData.Width == _screenWidth && _settingsData.Height == _screenHeight && _settingsData.Fullscreen == _fullscreen;
        }

        public override void SetResolutionData(int width, int height, bool fullscreen)
        {
            _screenWidth = width;
            _screenHeight = height;
            _fullscreen = fullscreen;

            CallOnChangeSettingsEvent();
        }

        protected override (int, int)[] GetIntDataList()
        {
            return new (int, int)[]
            {
                ((int)_motionBlur.quality.value , _settingsData.MotionBlurQuality),
                (QualitySettings.GetQualityLevel() , _settingsData.QualityLevel),
                (_screenWidth , _settingsData.Width),
                (_screenHeight , _settingsData.Height),
                (Application.targetFrameRate, _settingsData.TargetFrameRate)
            };
        }

        protected override (bool, bool)[] GetBoolDataList()
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

        protected override void SetData()
        {
            base.SetData();

            _screenWidth = _settingsData.Width;
            _screenHeight = _settingsData.Height;
            _fullscreen = _settingsData.Fullscreen;

            Screen.SetResolution(_screenWidth, _screenHeight, _fullscreen);
        }
    }
}