using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct SaveInfo
    {
        public long LastUpdated;
        public string Name;

        public SaveInfo(long lastUpdated, string name) : this()
        {
            LastUpdated = lastUpdated;
            Name = name;
        }
    }
}