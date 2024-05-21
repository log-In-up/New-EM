using Assets.Scripts.Data;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public GameData CurrentGameData { get; set; }

        public ObservableDictionary<string, SaveInfo> ObservableDataSlots { get; set; }

        public PersistentProgressService()
        {
            ObservableDataSlots = new ObservableDictionary<string, SaveInfo>();
        }
    }
}