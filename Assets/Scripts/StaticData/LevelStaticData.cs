using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "Level Static Data", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [SerializeField]
        private List<EnemySpawnData> _enemySpawnData;

        [SerializeField]
        private Vector3 _initialPlayerPosition;

        [SerializeField]
        private Quaternion _initialPlayerRotation;

        [SerializeField]
        private string _levelName;

        [SerializeField]
        private string _playerSpawnPointTag = "PlayerStartingPoint";

        public List<EnemySpawnData> EnemySpawnData { get => _enemySpawnData; set => _enemySpawnData = value; }
        public Vector3 InitialPlayerPosition { get => _initialPlayerPosition; set => _initialPlayerPosition = value; }
        public Quaternion InitialPlayerRotation { get => _initialPlayerRotation; set => _initialPlayerRotation = value; }
        public string LevelName { get => _levelName; set => _levelName = value; }
        public string PlayerSpawnPointTag => _playerSpawnPointTag;
    }
}