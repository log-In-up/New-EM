﻿using Assets.Scripts.UserInterface;

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
        void InitializeWindows(ServiceLocator serviceLocator);

        /// <summary>
        /// Opens dialog window - <see cref="dialogWindowID"/>.
        /// </summary>
        /// <param name="dialogWindowID">The ID of the dialog box to open.</param>
        void OpenDialogWindow(DialogWindowID dialogWindowID);
    }
}