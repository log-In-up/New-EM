using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public string CurrentLevel;
        public SaveInfo SaveInfo;

        public GameData()
        {
            CurrentLevel = "Main";
        }
    }
}