using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.PauseAndContinue
{
    public class PauseContinueService : IPauseContinueService
    {
        private const float NORMAL_TIME_SCALE = 1.0f;
        private const float STOPPED_TIME_SCALE = 0.0f;

        private bool _isPaused;

        public PauseContinueService()
        {
            _isPaused = false;
        }

        public bool IsPaused => _isPaused;

        public void Continue()
        {
            Time.timeScale = NORMAL_TIME_SCALE;
            AudioListener.pause = false;

            _isPaused = false;
        }

        public void Pause()
        {
            Time.timeScale = STOPPED_TIME_SCALE;
            AudioListener.pause = true;

            _isPaused = true;
        }
    }
}