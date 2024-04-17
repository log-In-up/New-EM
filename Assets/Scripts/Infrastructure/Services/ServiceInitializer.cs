using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.PauseAndContinue;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.Settings;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.StaticData;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services
{
    public class ServiceInitializer
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ServiceLocator _serviceLocator;
        private readonly GameStaticData _gameStaticData;
        private readonly ISceneLoader _sceneLoader;

        public ServiceInitializer(GameStateMachine stateMachine,
            ServiceLocator serviceLocator,
            GameStaticData gameStaticData,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _serviceLocator = serviceLocator;
            _gameStaticData = gameStaticData;
            _sceneLoader = sceneLoader;
        }

        public async Task RegisterServicesAsync()
        {
            _serviceLocator.RegisterService<IGameStateMachine>(_stateMachine);
            _serviceLocator.RegisterService<IInputService>(new InputService());
            _serviceLocator.RegisterService<IPauseContinueService>(new PauseContinueService());
            _serviceLocator.RegisterService(_sceneLoader);

            await RegisterAssetProviderAsync();
            RegisteringSettingsServices();
            await RegisterStaticDataAsync();

            _serviceLocator.RegisterService<IGameFactory>(new GameFactory(
                _serviceLocator.GetService<IAssetProvider>(),
                _serviceLocator.GetService<IStaticDataService>()));

            _serviceLocator.RegisterService<IPersistentProgressService>(new PersistentProgressService());

            await RegisterSaveLoadServiceAsync();
        }

        private async void RegisteringSettingsServices()
        {
            ICameraService cameraService = new CameraService();
            IAssetProvider assetProvider = _serviceLocator.GetService<IAssetProvider>();
            IAudioService audioService = new AudioService(assetProvider, _gameStaticData.AudioMixerReference);
            IGraphicsService graphicsService = new GraphicsService(assetProvider, _gameStaticData.VolumeProfileReference);

            ISettingsService gameSettingsService = new GameSettingsService(
                audioService, cameraService, graphicsService,
                Application.persistentDataPath,
                _gameStaticData.SettingsFileName);

            await gameSettingsService.Initialize();

            _serviceLocator.RegisterService(audioService);
            _serviceLocator.RegisterService(cameraService);
            _serviceLocator.RegisterService(graphicsService);
            _serviceLocator.RegisterService(gameSettingsService);
        }

        private async Task RegisterSaveLoadServiceAsync()
        {
            ISaveLoadService saveLoadService = new SaveLoadService(
                    _serviceLocator.GetService<IGameFactory>(),
                    _serviceLocator.GetService<IPersistentProgressService>(),
                    Application.persistentDataPath,
                    _gameStaticData.SaveFileName,
                    _gameStaticData.EncryptionCodeWord);

            await saveLoadService.Initialize();

            _serviceLocator.RegisterService(saveLoadService);
        }

        private async Task RegisterAssetProviderAsync()
        {
            IAssetProvider assetProvider = new AssetProvider();
            await assetProvider.Initialize();

            _serviceLocator.RegisterService(assetProvider);
        }

        private async Task RegisterStaticDataAsync()
        {
            IStaticDataService staticDataService = new StaticDataService(
                _serviceLocator.GetService<IAssetProvider>(),
                _gameStaticData);

            _serviceLocator.RegisterService(staticDataService);
            await staticDataService.LoadDataAsync();
        }
    }
}