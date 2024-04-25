using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.SaveLoad;

namespace Assets.Scripts.UserInterface.DialogueScreens
{
    public class OverwriteSaveWindow : WarningWindow
    {
        private string _saveToOverwrite;
        private ISaveLoadService _saveLoadService;
        public override DialogWindowID ID => DialogWindowID.OverwriteSave;

        public override void SendPayload<TPayload>(TPayload payload)
        {
            _saveToOverwrite = payload.ToString();
        }

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _saveLoadService = serviceLocator.GetService<ISaveLoadService>();
        }

        protected override async void OnClickPositive()
        {
            await _saveLoadService.Save(_saveToOverwrite);

            base.OnClickPositive();
        }
    }
}