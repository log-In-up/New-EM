using Assets.Scripts.UserInterface;

namespace Assets.Scripts.Infrastructure.Services.UserInterface
{
    /// <summary>
    /// A service for interacting with user interface screens.
    /// </summary>
    public interface IGameUI : IService
    {
        /// <summary>
        /// Removes all objects from the screen stack.
        /// </summary>
        void ClearScreens();

        /// <summary>
        /// Prepares user interface screens for use.
        /// </summary>
        /// <param name="serviceLocator">In-game services locator.</param>
        void InitializeScreens(ServiceLocator serviceLocator);

        /// <summary>
        /// Opens the user interface screen - <see cref="screenID"/>.
        /// </summary>
        /// <param name="screenID">ID of the screen to open.</param>
        void OpenScreen(ScreenID screenID);

        /// <summary>
        /// Opens the user interface screen - <see cref="screenID"/>.
        /// </summary>
        /// <typeparam name="TPayload">Payload type.</typeparam>
        /// <param name="screenID">ID of the screen to open.</param>
        /// <param name="payload">Payload at open screen.</param>
        void OpenScreen<TPayload>(ScreenID screenID, TPayload payload) where TPayload : class;

        /// <summary>
        /// Returns the previous screen from the screen stack.
        /// </summary>
        /// <returns>Previous screen.</returns>
        ScreenID PeekScreen();

        /// <summary>
        /// Returns and removes the previous screen from the screen stack.
        /// </summary>
        /// <returns>Previous screen.</returns>
        ScreenID PopScreen();

        /// <summary>
        /// Adds a screen to the screen stack.
        /// </summary>
        /// <param name="screenID">ID of the screen to add.</param>
        void PushScreen(ScreenID screenID);
    }
}