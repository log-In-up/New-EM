using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.UserInterface;
using System;

namespace Assets.Scripts.Infrastructure.Services.UserInterface
{
    /// <summary>
    /// A service for interacting with user interface dialog windows.
    /// </summary>
    public interface IGameDialogUI : IService
    {
        /// <summary>
        /// Closes all dialog windows.
        /// </summary>
        void CloseDialogWindows();

        /// <summary>
        /// Prepares dialog windows for use.
        /// </summary>
        /// <param name="serviceLocator">In-game services locator.</param>
        void InitializeWindows(IServiceLocator serviceLocator);

        /// <summary>
        /// Opens dialog window - <see cref="dialogWindowID"/>.
        /// </summary>
        /// <param name="dialogWindowID">The ID of the dialog box to open.</param>
        void OpenDialogWindow(DialogWindowID dialogWindowID);

        /// <summary>
        /// Opens dialog window - <see cref="dialogWindowID"/>.
        /// </summary>
        /// <typeparam name="TPayload">Payload type.</typeparam>
        /// <param name="dialogWindowID">The ID of the dialog box to open.</param>
        /// <param name="payload">Payload at open window.</param>
        void OpenDialogWindow<TPayload>(DialogWindowID dialogWindowID, TPayload payload) where TPayload : class;

        /// <summary>
        /// Adds a dialog box action to handle internal actions.
        /// </summary>
        /// <param name="dialogWindowID">ID of the dialog box to add actions to.</param>
        /// <param name="onCancel">Action of the dialog box on the Cancel event.</param>
        /// <param name="onApply">Action of the dialog box on the Apply event.</param>
        void AddWindowActions(DialogWindowID dialogWindowID, Action onCancel, Action onApply);

        /// <summary>
        /// Is there currently an active window?
        /// </summary>
        bool IsActive { get; }
    }
}