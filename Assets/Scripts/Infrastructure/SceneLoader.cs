using Assets.Scripts.StaticData;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameStaticData _gameStaticData;

        public SceneLoader(ICoroutineRunner coroutineRunner, GameStaticData gameStaticData)
        {
            _coroutineRunner = coroutineRunner;
            _gameStaticData = gameStaticData;
        }

        public void Load(string sceneName, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(sceneName, onLoaded));
        }

        public AsyncOperation LoadGameLevel(string sceneName, Action onLoaded = null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            _coroutineRunner.StartCoroutine(LoadScene(asyncOperation, onLoaded));

            return asyncOperation;
        }

        public void LoadScreensaverScene(Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(_gameStaticData.GameScreensaver, onLoaded));
        }

        private IEnumerator LoadScene(string sceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);

            while (!waitNextScene.isDone)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }

        private IEnumerator LoadScene(AsyncOperation asyncOperation, Action onLoaded = null)
        {
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}