using Assets.Scripts.Infrastructure.Services;
using System.Threading.Tasks;

namespace Assets.Scripts.StaticData
{
    public interface IStaticDataService : IService
    {
        EnemyStaticData GetEnemyData(EnemyTypeId typeId);

        LevelStaticData GetLevelData(string sceneKey);

        Task LoadDataAsync();
    }
}