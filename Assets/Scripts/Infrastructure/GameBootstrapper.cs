using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    [DisallowMultipleComponent]
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game(this);

            DontDestroyOnLoad(gameObject);

            _game.Launch();
        }

        private void OnApplicationQuit() =>
            _game = null;
    }
}