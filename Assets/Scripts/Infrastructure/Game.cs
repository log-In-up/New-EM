using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.States;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        private const string InitialScene = "Initial";
        private SceneLoader _sceneLoader;
        private ServiceInitializer _serviceInitializer;
        private ServiceLocator _serviceLocator;
        private GameStateMachine _stateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            _sceneLoader = new SceneLoader(coroutineRunner);
            _serviceLocator = new ServiceLocator();
            _serviceInitializer = new ServiceInitializer(_stateMachine, _serviceLocator);
            _stateMachine = new GameStateMachine(_sceneLoader, _serviceLocator);
        }

        ~Game()
        {
            _sceneLoader = null;
            _serviceLocator = null;
            _serviceInitializer = null;
            _stateMachine = null;
        }

        public async void Launch()
        {
            await _serviceInitializer.RegisterServicesAsync();

            _stateMachine.InitializeStateMashine();

            _sceneLoader.Load(InitialScene, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}