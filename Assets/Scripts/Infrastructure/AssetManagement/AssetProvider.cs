using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private Dictionary<string, AsyncOperationHandle> _cleanIgnoreOperationsCache;
        private Dictionary<string, AsyncOperationHandle> _completedOperationsCache;
        private Dictionary<string, List<AsyncOperationHandle>> _handlesCache;

        public AssetProvider()
        {
            _cleanIgnoreOperationsCache = new Dictionary<string, AsyncOperationHandle>();
            _completedOperationsCache = new Dictionary<string, AsyncOperationHandle>();
            _handlesCache = new Dictionary<string, List<AsyncOperationHandle>>();
        }

        ~AssetProvider()
        {
            CleanUp();
            CleanUpIgnoredCashe();

            _cleanIgnoreOperationsCache = null;
            _completedOperationsCache = null;
            _handlesCache = null;
        }

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handlesCache.Values)
            {
                foreach (AsyncOperationHandle handle in resourceHandles)
                {
                    Addressables.Release(handle);
                }
            }

            _completedOperationsCache.Clear();
            _handlesCache.Clear();
        }

        public async Task Initialize()
        {
            await Addressables.InitializeAsync().Task;
        }

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedOperationsCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle operationHandle))
                return operationHandle.Result as T;

            return await RunWithCasheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(IResourceLocation resourceLocation) where T : class
        {
            if (_completedOperationsCache.TryGetValue(resourceLocation.PrimaryKey, out AsyncOperationHandle operationHandle))
                return operationHandle.Result as T;

            return await RunWithCasheOnComplete(Addressables.LoadAssetAsync<T>(resourceLocation.PrimaryKey), resourceLocation.PrimaryKey);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedOperationsCache.TryGetValue(address, out AsyncOperationHandle operationHandle))
                return operationHandle.Result as T;

            return await RunWithCasheOnComplete(Addressables.LoadAssetAsync<T>(address), address);
        }

        public async Task<IList<IResourceLocation>> LoadByLabel(string label, Type type)
        {
            return await Addressables.LoadResourceLocationsAsync(label, type).Task;
        }

        public async Task<T> LoadWithoutCleaning<T>(AssetReference assetReference) where T : class
        {
            if (_cleanIgnoreOperationsCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle operationHandle))
                return operationHandle.Result as T;

            AsyncOperationHandle asyncOperation = Addressables.LoadAssetAsync<T>(assetReference);
            await asyncOperation.Task;

            _cleanIgnoreOperationsCache.Add(assetReference.AssetGUID, asyncOperation);

            return asyncOperation.Result as T;
        }

        private void AddHandle<T>(AsyncOperationHandle<T> asyncOperationHandle, string key) where T : class
        {
            if (!_handlesCache.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new List<AsyncOperationHandle>();
                _handlesCache[key] = handles;
            }
            handles.Add(asyncOperationHandle);
        }

        private void CleanUpIgnoredCashe()
        {
            foreach (KeyValuePair<string, AsyncOperationHandle> item in _cleanIgnoreOperationsCache)
            {
                Addressables.Release(item.Value);
            }
        }

        private async Task<T> RunWithCasheOnComplete<T>(AsyncOperationHandle<T> asyncOperationHandle, string casheKey) where T : class
        {
            asyncOperationHandle.Completed += handle => _completedOperationsCache[casheKey] = handle;

            AddHandle(asyncOperationHandle, casheKey);

            return await asyncOperationHandle.Task;
        }
    }
}