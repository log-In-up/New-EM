using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class SettingsData
    {
        public bool Bloom, DepthOfField, MotionBlur, Vignette;
        public float FieldOfView, Gamma, MasterVolume;
        public int MotionBlurQuality, QualityLevel, TargetFrameRate;

        public SettingsData()
        {
            Bloom = true;
            DepthOfField = true;
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