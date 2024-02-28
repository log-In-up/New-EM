namespace Assets.Scripts.Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        delegate void CancelEventHandler();

        event CancelEventHandler OnClickCancel;
    }
}