//namespace Assets.Scripts.Infrastructure.Services
//{
using Assets.Scripts.Infrastructure.Services;

namespace Custom
{
    /// <summary>
    /// In-game services locator.
    /// </summary>
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        public ServiceLocator()
        {
            _instance = this;
        }

        /// <summary>
        /// Container with an in-game service locator.
        /// </summary>
        public static ServiceLocator Container => _instance ??= new ServiceLocator();

        /// <summary>
        /// Returns the in-game service.
        /// </summary>
        /// <typeparam name="TService">Type of in-game service.</typeparam>
        /// <returns>In-game service.</returns>
        public TService GetService<TService>() where TService : IService
        {
            return Implementation<TService>.ServiceInstance;
        }

        /// <summary>
        /// Registers an in-game service.
        /// </summary>
        /// <typeparam name="TService">Type of in-game service.</typeparam>
        /// <param name="implementation">Implementation of an in-game service.</param>
        public void RegisterService<TService>(TService implementation) where TService : IService
        {
            Implementation<TService>.ServiceInstance = implementation;
        }

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}