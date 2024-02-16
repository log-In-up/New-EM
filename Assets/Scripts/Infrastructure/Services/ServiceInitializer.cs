using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.StaticData;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services
{
    public class ServiceInitializer
    {
        private const string SaveFileName = "Save.txt";
        private const string ENCRYPTION_CODE_WORD = "EMEncrypt";
        private readonly GameStateMachine _stateMachine;
        private readonly ServiceLocator _serviceLocator;

        public ServiceInitializer(GameStateMachine stateMachine, ServiceLocator serviceLocator)
        {
            _stateMachine = stateMachine;
            _serviceLocator = serviceLocator;
        }

        public async Task RegisterServicesAsync()
        {
            _serviceLocator.RegisterService<IGameStateMachine>(_stateMachine);

            RegisterAssetProvider();
            await RegisterStaticDataAsync();

            _serviceLocator.RegisterService<IGameFactory>(new GameFactory(
                _serviceLocator.GetService<IAssetProvider>(),
                _serviceLocator.GetService<IStaticDataService>()));

            _serviceLocator.RegisterService<IPersistentProgressService>(new PersistentProgressService());

            _serviceLocator.RegisterService<ISaveLoadService>(new SaveLoadService(
                _serviceLocator.GetService<IGameFactory>(),
                _serviceLocator.GetService<IPersistentProgressService>(),
                Application.persistentDataPath,
                SaveFileName,
                ENCRYPTION_CODE_WORD));
        }

        private void RegisterAssetProvider()
        {
            IAssetProvider assetProvider = new AssetProvider();
            assetProvider.Initialize();

            _serviceLocator.RegisterService<IAssetProvider>(assetProvider);
        }

        private async Task RegisterStaticDataAsync()
        {
            IStaticDataService staticDataService = new StaticDataService(_serviceLocator.GetService<IAssetProvider>());

            _serviceLocator.RegisterService(staticDataService);
            await staticDataService.LoadDataAsync();
        }
    }
}