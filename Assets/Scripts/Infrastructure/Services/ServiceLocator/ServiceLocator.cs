using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.ServicesLocator
{
    public class ServiceLocator : IServiceLocator
    {
        private static ServiceLocator _instance;
        private ConcurrentDictionary<Type, object> _services = new();

        public ServiceLocator()
        {
            _instance = this;
            _services = new ConcurrentDictionary<Type, object>();
        }

        ~ServiceLocator()
        {
            _services.Clear();
            _services = null;
        }

        public static IServiceLocator Container => _instance ??= new ServiceLocator();

        public T GetService<T>() where T : IService
        {
            Type type = typeof(T);
            return (T)GetService(type);
        }

        public object GetService(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!_services.TryGetValue(type, out object value))
            {
                throw new KeyNotFoundException($"Service of type {type} is not registered");
            }

            return value;
        }

        public bool IsRegisteredService<T>() where T : IService
        {
            Type type = typeof(T);
            return IsRegisteredService(type);
        }

        public bool IsRegisteredService(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return _services.ContainsKey(type);
        }

        public void RegisterService<T>(T service) where T : IService
        {
            Type type = typeof(T);
            RegisterService(type, service);
        }

        public void RegisterService(Type type, object service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (!type.IsAssignableFrom(service.GetType()))
            {
                throw new ArgumentException($"The given service is not assignable from the given type {type}");
            }
            if (!_services.TryAdd(type, service))
            {
                throw new ArgumentException($"Service of type {type} is already registered");
            }
        }

        public bool UnregisterService<T>() where T : IService
        {
            Type type = typeof(T);
            return UnregisterService(type);
        }

        public bool UnregisterService(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return _services.TryRemove(type, out _);
        }
    }
}