using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UserInterface
{
    public interface IGameUI : IService
    {
        void ClearScreens();

        void Initialize(ServiceLocator serviceLocator);

        void OpenScreen(WindowID screenID);

        WindowID PeekScreen();

        void PushScreen(WindowID screenID);
    }
}