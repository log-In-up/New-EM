using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    [DisallowMultipleComponent]
    public class GameRunner : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrapper _gameBootstrapper = null;

        private void Awake()
        {
            GameBootstrapper bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper == null)
            {
                Instantiate(_gameBootstrapper);
            }
        }
    }
}