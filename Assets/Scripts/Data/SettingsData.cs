using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class SettingsData
    {
        public float MasterVolume;

        public SettingsData()
        {
            MasterVolume = 1.0f;
        }
    }
}