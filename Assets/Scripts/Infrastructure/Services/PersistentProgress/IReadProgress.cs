using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    public interface IReadProgress
    {
        void LoadProgress(GameData gameData);
    }
}