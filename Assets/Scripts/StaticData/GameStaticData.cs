using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "Game Static Data", menuName = "Static Data/Game")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Encryption codeword for the save/load system. Leave blank to disable encryption.\r\nNote: This only works with new save files.")]
        private string _encryptionCodeWord = "EMEncrypt";

        [SerializeField]
        [Tooltip("The name of the scene with a dynamic splash screen.")]
        private string _gameScreensaver = "GameScreensaver";

        [SerializeField]
        [Tooltip("The name of the scene to initialize the game.")]
        private string _initialScene = "Initial";

        [SerializeField]
        [Tooltip("The name of the file to save (including extension).")]
        private string _saveFileName = "Save.txt";

        [SerializeField]
        private string _enemyStaticDataLabel = "EnemyStaticData";

        [SerializeField]
        private string _levelStaticDataLabel = "LevelStaticData";

        public string EncryptionCodeWord => _encryptionCodeWord;
        public string GameScreensaver => _gameScreensaver;
        public string InitialScene => _initialScene;
        public string SaveFileName => _saveFileName;
        public string EnemyStaticDataLabel => _enemyStaticDataLabel;
        public string LevelStaticDataLabel => _levelStaticDataLabel;
    }
}