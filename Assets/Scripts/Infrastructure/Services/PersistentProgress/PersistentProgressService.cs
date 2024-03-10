using Assets.Scripts.Data;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public GameData CurrentGameData { get; set; }

        public ObservableDictionary<string, GameData> ObservableDataProfiles { get; set; }

        public PersistentProgressService()
        {
            ObservableDataProfiles = new ObservableDictionary<string, GameData>();
        }
    }
}