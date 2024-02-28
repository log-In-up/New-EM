using Assets.Scripts.Infrastructure.Services;
using System;

namespace Assets.Scripts.Infrastructure
{
    public interface ISceneLoader : IService
    {
        void Load(string sceneName, Action onLoaded = null);

        void LoadScreensaverScene(Action onLoaded = null);
    }
}