using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "Game Static Data", menuName = "Static Data/Game")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField]
        private AssetReference _audioMixerReference;

        [SerializeField]
        [Tooltip("Encryption codeword for the save/load system. Leave blank to disable encryption.\r\nNote: This only works with new save files.")]
        private string _encryptionCodeWord = "EMEncrypt";

        [SerializeField]
        private AssetLabelReference _enemyStaticDataLabel;

        [SerializeField]
        [Tooltip("Scene with a dynamic splash screen.")]
        private SceneAsset _gameScreensaver;

        [SerializeField]
        [Tooltip("Scene for initializing the game.")]
        private SceneAsset _initialScene;

        [SerializeField]
        private AssetLabelReference _levelStaticDataLabel;

        [SerializeField]
        [Tooltip("The name of the file to save (including extension).")]
        private string _saveFileName = "Save.txt";

        [SerializeField]
        [Tooltip("Game settings file name (including extension).")]
        private string _settingsFileName = "SettingsData.txt";

        public AssetReference AudioMixerReference => _audioMixerReference;
        public string EncryptionCodeWord => _encryptionCodeWord;
        public AssetLabelReference EnemyStaticDataLabel => _enemyStaticDataLabel;
        public string GameScreensaver => _gameScreensaver.name;
        public string InitialScene => _initialScene.name;
        public AssetLabelReference LevelStaticDataLabel => _levelStaticDataLabel;
        public string SaveFileName => _saveFileName;
        public string SettingsFileName => _settingsFileName;
    }
}