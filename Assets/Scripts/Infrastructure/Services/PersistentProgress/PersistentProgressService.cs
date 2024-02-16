using Assets.Scripts.Data;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public Dictionary<string, GameData> DataProfiles { get; set; }
        public GameData GameData { get; set; }
    }
}