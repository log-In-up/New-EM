using Assets.Scripts.Data;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        GameData Load(string profileId);

        Dictionary<string, GameData> LoadAllProfiles();

        void Save(string profileId);
    }
}