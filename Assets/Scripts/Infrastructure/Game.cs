using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        private const string InitialScene = "Initial";
        private readonly GameUI _hud;
        private SceneLoader _sceneLoader;
        private ServiceInitializer _serviceInitializer;
        private ServiceLocator _serviceLocator;
        private GameStateMachine _stateMachine;

        public Game(ICoroutineRunner coroutineRunner, GameUI hud)
        {
            _sceneLoader = new SceneLoader(coroutineRunner);
            _serviceLocator = new ServiceLocator();
            _stateMachine = new GameStateMachine(_sceneLoader, _serviceLocator);
            _serviceInitializer = new ServiceInitializer(_stateMachine, _serviceLocator);
            _hud = hud;
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
            GameUI hud = CreateAndRegisterHUD();
            hud.OpenScreen(WindowID.Title);

            await _serviceInitializer.RegisterServicesAsync();
            _stateMachine.InitializeStateMashine();

            hud.Initialize();

            _sceneLoader.Load(InitialScene, EnterLoadLevel);
        }

        private GameUI CreateAndRegisterHUD()
        {
            GameUI hud = Object.Instantiate(_hud);
            Object.DontDestroyOnLoad(hud);
            _serviceLocator.RegisterService<IGameUI>(hud);

            return hud;
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}