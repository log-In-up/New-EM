using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.Factory
{
    /// <summary>
    /// The interface is responsible for initializing components on the enemy.
    /// </summary>
    /// <typeparam name="T">Type of enemy static data.</typeparam>
    public interface IEnemyConstructor<T> where T : EnemyStaticData
    {
        /// <summary>
        /// Initializing the component on the enemy.
        /// </summary>
        /// <param name="data"></param>
        void Construct(T data);
    }
}