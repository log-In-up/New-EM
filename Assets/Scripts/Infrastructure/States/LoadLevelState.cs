using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using Assets.Scripts.UserInterface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameUI _gameUI;

        public LoadLevelState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IStaticDataService staticDataService,
            IGameUI gameUI)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _gameUI = gameUI;
        }

        public void Enter(string payload)
        {
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();

            _gameUI.OpenScreen(WindowID.Loading);
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit()
        {
        }

        private void InformProgressReaders()
        {
            foreach (IReadProgress progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.GameData);
            }
        }

        private async void InitGameWorld()
        {
            LevelStaticData levelStaticData = LevelStaticData();

            InitSpawners(levelStaticData);

            GameObject player = await _gameFactory.CreatePlayer(levelStaticData.InitialPlayerPosition, levelStaticData.InitialPlayerRotation);
        }

        private void InitSpawners(LevelStaticData levelStaticData)
        {
            foreach (EnemySpawnData spawnData in levelStaticData.EnemySpawnData)
            {
                _gameFactory.CreateSpawner(spawnData);
            }
        }

        private LevelStaticData LevelStaticData()
        {
            return _staticDataService.GetLevelData(SceneManager.GetActiveScene().name);
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }
    }
}