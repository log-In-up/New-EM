using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    public interface ISaveProgress : IReadProgress
    {
        void UpdateProgress(GameData gameData);
    }
}