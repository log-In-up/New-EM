using Assets.Scripts.Data;
using Assets.Scripts.Utility;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.Services.Settings
{
    public class CameraService : ICameraService
    {
        private float _currentFieldOfView;
        private SettingsData _settingsData;

        public event ICameraService.CameraFOVChanged FieldOfViewChanged;

        public event IProcessSettings.SettingsChanged OnSettingsChanged;

        public bool DataAreEqual()
        {
            foreach ((float, float) item in GetFloatDataList())
            {
                if (item.Item1.ApproximatelyEqual(item.Item2, 0.001f))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public Task Initialize<SettingsType>(SettingsType settingsData) where SettingsType : SettingsData
        {
            _settingsData = settingsData;

            _currentFieldOfView = _settingsData.FieldOfView;

            return Task.CompletedTask;
        }

        public void Invert()
        {
            _settingsData.FieldOfView = _currentFieldOfView;

            FieldOfViewChanged?.Invoke(_currentFieldOfView);
        }

        public void SetFieldOfView(float fieldOfView)
        {
            _currentFieldOfView = fieldOfView;

            FieldOfViewChanged?.Invoke(fieldOfView);
            OnSettingsChanged?.Invoke();
        }

        public void WriteData()
        {
            _settingsData.FieldOfView = _currentFieldOfView;
        }

        private (float, float)[] GetFloatDataList()
        {
            return new (float, float)[]
            {
                (_currentFieldOfView , _settingsData.FieldOfView)
            };
        }
    }
}