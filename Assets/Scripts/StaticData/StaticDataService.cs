﻿using Assets.Scripts.Infrastructure.AssetManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string ENEMY_STATIC_DATA_LABEL = "EnemyStaticData";
        private const string LEVEL_STATIC_DATA_LABEL = "LevelStaticData";

        private readonly IAssetProvider _assetProvider;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemiesСache;
        private Dictionary<string, LevelStaticData> _levelsCache;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _enemiesСache = new Dictionary<EnemyTypeId, EnemyStaticData>();
            _levelsCache = new Dictionary<string, LevelStaticData>();
            _assetProvider = assetProvider;
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
            IList<IResourceLocation> locations = await _assetProvider.LoadByLabel(ENEMY_STATIC_DATA_LABEL, typeof(EnemyStaticData));

            foreach (IResourceLocation location in locations)
            {
                EnemyStaticData handle = await _assetProvider.Load<EnemyStaticData>(location);
                _enemiesСache.Add(handle.EnemyTypeId, handle);
            }

            locations = await _assetProvider.LoadByLabel(LEVEL_STATIC_DATA_LABEL, typeof(LevelStaticData));

            foreach (IResourceLocation location in locations)
            {
                LevelStaticData handle = await _assetProvider.Load<LevelStaticData>(location);
                _levelsCache.Add(handle.LevelName, handle);
            }
        }
    }
}