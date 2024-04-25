using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using UnityEngine;

namespace Assets.Scripts.UserInterface.Elements
{
    public abstract class SettingsTab : MonoBehaviour
    {
        public virtual void Setup(IServiceLocator serviceLocator)
        {
        }

        public virtual void SetSettings()
        {
        }
    }
}