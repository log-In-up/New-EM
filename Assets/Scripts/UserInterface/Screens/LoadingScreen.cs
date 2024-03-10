using Assets.Scripts.Infrastructure.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.Screens
{
    public class LoadingScreen : Screen
    {
        [SerializeField]
        private Slider _progressBar;

        private AsyncOperation _loading;
        private Coroutine _loadingProgress;

        public override ScreenID ID => ScreenID.Loading;

        public override void Activate()
        {
            _progressBar.value = 0;

            base.Activate();

            _loadingProgress = StartCoroutine(ShowLoadStatus());
        }

        public override void Deactivate()
        {
            StopCoroutine(_loadingProgress);

            base.Deactivate();

            _progressBar.value = 0;
        }

        public override void Setup(ServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);
        }

        public override void SendPayload<AsyncOperation>(AsyncOperation payload)
        {
            _loading = payload as UnityEngine.AsyncOperation;
        }

        private IEnumerator ShowLoadStatus()
        {
            while (!_loading.isDone)
            {
                _progressBar.value = _loading.progress;
                yield return null;
            }
        }
    }
}