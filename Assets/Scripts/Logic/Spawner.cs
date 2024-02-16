using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Logic.EnemySpawners
{
    public class Spawner : MonoBehaviour, ISaveProgress
    {
        private IGameFactory _gameFactory;
        public EnemyTypeId EnemyType { get; set; }
        public string Id { get; set; }

        public void Constructor(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void LoadProgress(GameData gameData)
        {
        }

        public void UpdateProgress(GameData gameData)
        {
        }
    }
}