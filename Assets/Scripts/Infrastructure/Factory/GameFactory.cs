using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;

            ProgressReaders = new List<IReadProgress>();
            ProgressWriters = new List<ISaveProgress>();
        }

        ~GameFactory()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public List<IReadProgress> ProgressReaders { get; }
        public List<ISaveProgress> ProgressWriters { get; }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();

            _assetProvider.CleanUp();
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
            EnemyStaticData enemyStaticData = _staticData.GetEnemyData(typeId);

            GameObject prefab = await _assetProvider.Load<GameObject>(enemyStaticData.PrefabReference);
            GameObject enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            foreach (IEnemyConstructor<EnemyStaticData> constructor in enemy.GetComponents<IEnemyConstructor<EnemyStaticData>>())
            {
                constructor.Construct(enemyStaticData);
            }

            return enemy;
        }

        public async Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Player);

            return InstantiateRegistered(prefab, position, rotation);
        }

        public async Task CreateSpawner(EnemySpawnData spawnData)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Spawner);

            Spawner spawner = InstantiateRegistered(prefab, spawnData.Position, spawnData.Rotation)
                .GetComponent<Spawner>();

            spawner.Constructor(this);
            spawner.Id = spawnData.Id;
            spawner.EnemyType = spawnData.Type;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetsAddress.Spawner);
            await _assetProvider.Load<GameObject>(AssetsAddress.Player);
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject gameObject = Object.Instantiate(prefab, position, rotation);

            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);

            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private void Register(IReadProgress reader)
        {
            if (reader is ISaveProgress writer)
            {
                ProgressWriters.Add(writer);
            }

            ProgressReaders.Add(reader);
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (IReadProgress progress in gameObject.GetComponentsInChildren<IReadProgress>())
            {
                Register(progress);
            }
        }
    }
}