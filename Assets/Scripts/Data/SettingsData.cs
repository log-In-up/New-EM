using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class SettingsData
    {
        public bool Bloom, DepthOfField, Fullscreen, MotionBlur, Vignette;
        public float FieldOfView, Gamma, MasterVolume;
        public int MotionBlurQuality, QualityLevel, ScreenHeight, ScreenWidth, TargetFrameRate;

        public SettingsData()
        {
            Bloom = true;
            DepthOfField = true;
#if UNITY_EDITOR
            Fullscreen = false;
#elif UNITY_ANDROID
            Fullscreen = false;
#else
            Fullscreen = true;
#endif
            MotionBlur = true;
            Vignette = true;
            FieldOfView = 60.0f;
            Gamma = 1.0f;
            MasterVolume = 1.0f;
            MotionBlurQuality = 0;
            QualityLevel = 0;
            TargetFrameRate = -1;
        }
    }
}