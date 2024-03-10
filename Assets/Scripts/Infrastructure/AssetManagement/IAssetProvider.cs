using Assets.Scripts.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.Infrastructure.AssetManagement
{
    /// <summary>
    /// Service for downloading in-game resources.
    /// </summary>
    public interface IAssetProvider : IService
    {
        /// <summary>
        /// Asset provider cleanup.
        /// </summary>
        void CleanUp();

        /// <summary>
        /// Initializing the asset provider.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loading an asset via <see cref="assetReference"/>.
        /// </summary>
        /// <typeparam name="T">Type of asset to load.</typeparam>
        /// <param name="assetReference">Link to the asset in Addressables.</param>
        /// <returns>Asset.</returns>
        Task<T> Load<T>(AssetReference assetReference) where T : class;

        /// <summary>
        /// Loading an asset via <see cref="resourceLocation"/>.
        /// </summary>
        /// <typeparam name="T">Type of asset to load.</typeparam>
        /// <param name="resourceLocation">Link to the asset in Addressables.</param>
        /// <returns>Asset.</returns>
        Task<T> Load<T>(IResourceLocation resourceLocation) where T : class;

        /// <summary>
        /// Loading an asset via <see cref="address"/>.
        /// </summary>
        /// <typeparam name="T">Type of asset to load.</typeparam>
        /// <param name="address">Link to the asset.</param>
        /// <returns>Asset.</returns>
        Task<T> Load<T>(string address) where T : class;

        /// <summary>
        /// Creates a list of resource locations using label and type.
        /// </summary>
        /// <param name="label">Label of assets.</param>
        /// <param name="type">Type of assets.</param>
        /// <returns>List of resource locations.</returns>
        Task<IList<IResourceLocation>> LoadByLabel(string label, Type type);
    }
}