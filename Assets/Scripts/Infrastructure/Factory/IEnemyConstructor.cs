using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IEnemyConstructor<T> where T : EnemyStaticData
    {
        void Construct(T data);
    }
}