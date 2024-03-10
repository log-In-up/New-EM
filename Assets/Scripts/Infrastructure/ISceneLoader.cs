using Assets.Scripts.Infrastructure.Services;
using System;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    /// <summary>
    /// Scene loading service.
    /// </summary>
    public interface ISceneLoader : IService
    {
        /// <summary>
        /// Loads the scene named <see cref="sceneName"/>.
        /// </summary>
        /// <param name="sceneName">The name of the scene stored in Scenes In Build.</param>
        /// <param name="onLoaded">Action performed after the scene is loaded.</param>
        void Load(string sceneName, Action onLoaded = null);

        /// <summary>
        /// Loads the scene named <see cref="sceneName"/>.
        /// </summary>
        /// <param name="sceneName">The name of the scene stored in Scenes In Build.</param>
        /// <param name="onLoaded">Action performed after the scene is loaded.</param>
        AsyncOperation LoadGameLevel(string sceneName, Action onLoaded = null);

        /// <summary>
        /// Loads the splash scene.
        /// </summary>
        /// <param name="onLoaded">Action performed after the scene is loaded.</param>
        void LoadScreensaverScene(Action onLoaded = null);
    }
}