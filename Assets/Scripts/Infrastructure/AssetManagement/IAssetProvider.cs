using Assets.Scripts.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        void CleanUp();

        void Initialize();

        Task<T> Load<T>(AssetReference assetReference) where T : class;

        Task<T> Load<T>(IResourceLocation resourceLocation) where T : class;

        Task<T> Load<T>(string address) where T : class;

        Task<IList<IResourceLocation>> LoadByLabel(string label, Type type);
    }
}