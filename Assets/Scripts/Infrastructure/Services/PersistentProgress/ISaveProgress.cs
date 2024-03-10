using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    /// <summary>
    /// The interface is responsible for write data into objects.
    /// </summary>
    public interface ISaveProgress : IReadProgress
    {
        /// <summary>
        /// Responsible for write data to <see cref="gameData"/>.
        /// </summary>
        /// <param name="gameData">Current game data.</param>
        void UpdateProgress(GameData gameData);
    }
}