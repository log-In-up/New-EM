using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    /// <summary>
    /// The interface is responsible for loading data into objects.
    /// </summary>
    public interface IReadProgress
    {
        /// <summary>
        /// Responsible for load data from <see cref="gameData"/>.
        /// </summary>
        /// <param name="gameData">Current game data.</param>
        void LoadProgress(GameData gameData);
    }
}