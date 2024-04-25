using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.Input;
using Assets.Scripts.Infrastructure.Services.PauseAndContinue;

namespace Assets.Scripts.UserInterface.Screens
{
    public class GameplayScreen : Screen
    {
        private IInputService _inputService;
        private IPauseContinueService _pauseContinueService;

        public override ScreenID ID => ScreenID.Gameplay;

        public override void Activate()
        {
            GameUI.ClearScreens();
            GameUI.PushScreen(ID);

            _inputService.OnClickCancel += OnClickCancel;

            base.Activate();
        }

        public override void Deactivate()
        {
            _inputService.OnClickCancel -= OnClickCancel;

            base.Deactivate();
        }

        public override void Setup(IServiceLocator serviceLocator)
        {
            base.Setup(serviceLocator);

            _inputService = serviceLocator.GetService<IInputService>();
            _pauseContinueService = serviceLocator.GetService<IPauseContinueService>();
        }

        private void OnClickCancel()
        {
            _pauseContinueService.Pause();
            GameUI.OpenScreen(ScreenID.GamePauseScreen);
        }
    }
}