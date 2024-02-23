using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UserInterface
{
    public interface IGameUI : IService
    {
        void OpenScreen(WindowID screenID);
    }
}