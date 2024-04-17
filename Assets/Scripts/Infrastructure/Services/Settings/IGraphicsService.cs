namespace Assets.Scripts.Infrastructure.Services.Settings
{
    /// <summary>
    /// The service is responsible for working with game graphics.
    /// </summary>
    public interface IGraphicsService : IProcessSettings
    {
        /// <summary>
        /// Applies screen resolution data.
        /// </summary>
        void ApplyResolutionData();

        /// <summary>
        /// Enables/disables blooming.
        /// </summary>
        /// <param name="active">On/Off switch.</param>
        void SetBloom(bool active);

        /// <summary>
        /// Enables/disables depth of field.
        /// </summary>
        /// <param name="active">On/Off switch.</param>
        void SetDepthOfField(bool active);

        /// <summary>
        /// Changes the gamma value.
        /// </summary>
        /// <param name="gamma">Gamma value.</param>
        void SetGamma(float gamma);

        /// <summary>
        /// Sets the target frame rate.
        /// </summary>
        /// <param name="maxFPS">Target frame rate.</param>
        void SetMaxFPS(int maxFPS);

        /// <summary>
        /// Adjusts motion blur.
        /// </summary>
        /// <param name="active">On/Off switch.</param>
        /// <param name="motionBlurQuality">Quality of motion blur.</param>
        void SetMoutionBlur(bool active, int motionBlurQuality);

        /// <summary>
        /// Sets the level of graphics quality settings.
        /// </summary>
        /// <param name="qualityLevel">Level of quality settings.</param>
        void SetQualityLevel(int qualityLevel);

        /// <summary>
        /// Reports screen resolution data.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="fullscreen">Is the game in full screen mode?</param>
        void SetResolutionData(int width, int height, bool fullscreen);

        /// <summary>
        /// Enables/disables vignette.
        /// </summary>
        /// <param name="active">On/Off switch.</param>
        void SetVignette(bool active);
    }
}