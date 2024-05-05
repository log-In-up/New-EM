using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.Settings;
using UnityEngine;

namespace Assets.Scripts.UserInterface.Elements
{
    public abstract class SettingsTab : MonoBehaviour
    {
        protected ISettingsService _settingsService;

#if UNITY_ANDROID || UNITY_IOS
        protected HandheldSettingsData _settingsData;
#elif UNITY_PS3 || UNITY_PS4 || UNITY_SAMSUNGTV || UNITY_XBOX360 || UNITY_XBOXONE
        protected ConsoleSettingsData _settingsData;
#elif UNITY_STANDALONE
        protected DesktopSettingsData _settingsData;
#endif

        public virtual void Setup(IServiceLocator serviceLocator)
        {
            _settingsService = serviceLocator.GetService<ISettingsService>();

#if UNITY_ANDROID || UNITY_IOS
            _settingsData = _settingsService.GetData<HandheldSettingsData>();
#elif UNITY_PS3 || UNITY_PS4 || UNITY_SAMSUNGTV || UNITY_XBOX360 || UNITY_XBOXONE
            _settingsData = _settingsService.GetData<ConsoleSettingsData>();
#elif UNITY_STANDALONE
            _settingsData = _settingsService.GetData<DesktopSettingsData>();
#endif
        }

        public virtual void SetSettings()
        {
        }
    }
}