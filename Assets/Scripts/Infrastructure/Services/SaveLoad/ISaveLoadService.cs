using Assets.Scripts.Data;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void CreateNewGame();

        void Delete(string profileId);

        string GetMostRecentlyUpdatedProfileId();

        string GetPath(string profileId);

        GameData Load(string profileId);

        Dictionary<string, GameData> LoadAllProfiles();

        void LoadRecentlyUpdatedSave();

        void Save(string profileId);
    }
}