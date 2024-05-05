using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class HandheldGraphics : GraphicsService<HandheldSettingsData>
    {
        public HandheldGraphics(
            IAssetProvider assetProvider,
            AssetReference volumeProfileReference) : base(
                assetProvider,
                volumeProfileReference)
        {
        }
    }
}