using Assets.Scripts.Data;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        Dictionary<string, GameData> DataProfiles { get; set; }
        GameData GameData { get; set; }
    }
}