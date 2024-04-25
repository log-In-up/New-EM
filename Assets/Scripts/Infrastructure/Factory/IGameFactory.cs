using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    /// <summary>
    /// Service for creating objects in the game.
    /// </summary>
    public interface IGameFactory : IService
    {
        /// <summary>
        /// List of objects that can read progress.
        /// </summary>
        List<IReadProgress> ProgressReaders { get; }

        /// <summary>
        /// List of objects that can write progress.
        /// </summary>
        List<ISaveProgress> ProgressWriters { get; }

        /// <summary>
        /// Cleans up the game factory.
        /// </summary>
        void CleanUp();

        /// <summary>
        /// Creates an enemy.
        /// </summary>
        /// <param name="typeId">ID of enemy.</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Rotation at spawn.</param>
        /// <returns>Async task.</returns>
        Task<GameObject> CreateEnemy(EnemyTypeId typeId, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Creates a player.
        /// </summary>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Rotation at spawn.</param>
        /// <returns>Async task.</returns>
        Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation);

        /// <summary>
        /// Creating an enemy spawner.
        /// </summary>
        /// <param name="spawnData">Data for spawn.</param>
        /// <returns>Async task.</returns>
        Task CreateEnemySpawner(EnemySpawnData spawnData);

        /// <summary>
        /// Warms up the game factory.
        /// </summary>
        /// <returns>Async task.</returns>
        Task WarmUp();
    }
}