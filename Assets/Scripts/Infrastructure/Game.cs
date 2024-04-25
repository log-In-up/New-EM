using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.StaticData;
using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        private readonly GameStaticData _gameStaticData;
        private readonly GameUI _hud;
        private ISceneLoader _sceneLoader;
        private ServiceInitializer _serviceInitializer;
        private IServiceLocator _serviceLocator;
        private IGameStateMachine _stateMachine;

        public Game(ICoroutineRunner coroutineRunner, GameUI hud, GameStaticData gameStaticData)
        {
            _gameStaticData = gameStaticData;
            _hud = hud;

            _sceneLoader = new SceneLoader(coroutineRunner, gameStaticData);
            _serviceLocator = new ServiceLocator();
            _stateMachine = new GameStateMachine(_sceneLoader, _serviceLocator);
            _serviceInitializer = new ServiceInitializer(_stateMachine, _serviceLocator, gameStaticData, _sceneLoader);
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
            hud.OpenScreen(ScreenID.Title);

            await _serviceInitializer.RegisterServicesAsync();
            _stateMachine.InitializeStateMashine();

            hud.InitializeScreens(_serviceLocator);
            hud.InitializeWindows(_serviceLocator);

            _sceneLoader.Load(_gameStaticData.InitialScene, EnterLoadLevel);
        }

        public void Stopping()
        {
            _serviceInitializer.ClearRegisters();
        }

        private GameUI CreateAndRegisterHUD()
        {
            GameUI hud = Object.Instantiate(_hud);
            Object.DontDestroyOnLoad(hud);

            _serviceLocator.RegisterService<IGameUI>(hud);
            _serviceLocator.RegisterService<IGameDialogUI>(hud);

            return hud;
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}