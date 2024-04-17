namespace Assets.Scripts.Infrastructure.Services.Settings
{
    /// <summary>
    /// The service is responsible for working with the player's camera.
    /// </summary>
    public interface ICameraService : IProcessSettings
    {
        /// <summary>
        /// A delegate that passes the viewing angle value.
        /// </summary>
        /// <param name="value">Field of view in degrees.</param>
        delegate void CameraFOVChanged(float value);

        /// <summary>
        /// This event occurs when the field of view changes.
        /// </summary>
        event CameraFOVChanged FieldOfViewChanged;

        /// <summary>
        /// Sets the viewing angle.
        /// </summary>
        /// <param name="fieldOfView">Field of view in degrees.</param>
        void SetFieldOfView(float fieldOfView);
    }
}