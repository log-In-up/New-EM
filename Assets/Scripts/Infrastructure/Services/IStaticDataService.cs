using Assets.Scripts.Infrastructure.Services;
using System.Threading.Tasks;

namespace Assets.Scripts.StaticData
{
    /// <summary>
    /// Interface responsible for receiving static data.
    /// </summary>
    public interface IStaticDataService : IService
    {
        /// <summary>
        /// Returns enemy data by <paramref name="typeId"/>.
        /// </summary>
        /// <param name="typeId">Enemy ID type.</param>
        /// <returns>Static enemy data.</returns>
        EnemyStaticData GetEnemyData(EnemyTypeId typeId);

        /// <summary>
        /// Returns level data by <paramref name="sceneKey"/>.
        /// </summary>
        /// <param name="sceneKey">The name of the scene stored in Scenes In Build.</param>
        /// <returns>Static level data.</returns>
        LevelStaticData GetLevelData(string sceneKey);

        /// <summary>
        /// Loads all static data.
        /// </summary>
        /// <returns>Async task.</returns>
        Task LoadDataAsync();
    }
}