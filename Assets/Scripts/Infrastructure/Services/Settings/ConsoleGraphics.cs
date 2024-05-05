using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class ConsoleGraphics : GraphicsService<ConsoleSettingsData>
    {
        public ConsoleGraphics(
            IAssetProvider assetProvider,
            AssetReference volumeProfileReference) : base(
                assetProvider,
                volumeProfileReference)
        {
        }
    }
}