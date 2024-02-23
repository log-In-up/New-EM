using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    [DisallowMultipleComponent]
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField]
        private GameUI _hud;

        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _hud);

            DontDestroyOnLoad(gameObject);

            _game.Launch();
        }

        private void OnApplicationQuit() =>
            _game = null;
    }
}