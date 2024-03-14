using Assets.Scripts.Infrastructure.AssetManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameStaticData _gameStaticData;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemiesСache;
        private Dictionary<string, LevelStaticData> _levelsCache;

        public StaticDataService(IAssetProvider assetProvider, GameStaticData gameStaticData)
        {
            _assetProvider = assetProvider;
            _gameStaticData = gameStaticData;

            _enemiesСache = new Dictionary<EnemyTypeId, EnemyStaticData>();
            _levelsCache = new Dictionary<string, LevelStaticData>();
        }

        ~StaticDataService()
        {
            _enemiesСache.Clear();
            _enemiesСache = null;

            _levelsCache.Clear();
            _levelsCache = null;
        }

        public EnemyStaticData GetEnemyData(EnemyTypeId typeId)
        {
            if (_enemiesСache.TryGetValue(typeId, out EnemyStaticData enemy))
                return enemy;

            return null;
        }

        public LevelStaticData GetLevelData(string sceneKey)
        {
            if (_levelsCache.TryGetValue(sceneKey, out LevelStaticData levelData))
                return levelData;

            return null;
        }

        public async Task LoadDataAsync()
        {
            IList<IResourceLocation> locations = await _assetProvider.LoadByLabel(_gameStaticData.EnemyStaticDataLabel.labelString, typeof(EnemyStaticData));

            foreach (IResourceLocation location in locations)
            {
                EnemyStaticData handle = await _assetProvider.Load<EnemyStaticData>(location);
                _enemiesСache.Add(handle.EnemyTypeId, handle);
            }

            locations = await _assetProvider.LoadByLabel(_gameStaticData.LevelStaticDataLabel.labelString, typeof(LevelStaticData));

            foreach (IResourceLocation location in locations)
            {
                LevelStaticData handle = await _assetProvider.Load<LevelStaticData>(location);
                _levelsCache.Add(handle.LevelName, handle);
            }
        }
    }
}