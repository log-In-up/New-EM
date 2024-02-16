using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "Enemy Static Data", menuName = "Static Data/Enemies")]
    public class EnemyStaticData : ScriptableObject
    {
        [SerializeField]
        private EnemyTypeId _enemyTypeId;

        [SerializeField]
        private AssetReferenceGameObject _prefabReference;

        public EnemyTypeId EnemyTypeId => _enemyTypeId;

        public AssetReferenceGameObject PrefabReference => _prefabReference;
    }
}