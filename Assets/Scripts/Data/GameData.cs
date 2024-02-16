using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public string CurrentLevel;

        public GameData()
        {
            CurrentLevel = "Main";
        }
    }
}