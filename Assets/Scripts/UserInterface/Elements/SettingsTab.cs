using Assets.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Assets.Scripts.UserInterface.Elements
{
    public abstract class SettingsTab : MonoBehaviour
    {
        public virtual void Setup(ServiceLocator serviceLocator)
        {
        }

        public virtual void SetSettings()
        {
        }
    }
}