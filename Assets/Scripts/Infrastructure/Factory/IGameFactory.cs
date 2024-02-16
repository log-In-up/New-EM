using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<IReadProgress> ProgressReaders { get; }
        List<ISaveProgress> ProgressWriters { get; }

        void CleanUp();

        Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);

        Task<GameObject> CreateHUD();

        Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation);

        Task CreateSpawner(EnemySpawnData spawnData);

        Task WarmUp();
    }
}