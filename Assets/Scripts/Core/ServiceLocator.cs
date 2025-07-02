using UnityEngine;

namespace NotBubbleFall
{
    public interface IService { }

    public static class ServiceLocator
    {
        public static void Register<T>(T service) where T : class, IService
        {
            if (ServiceHelper<T>.Service == null) {
                ServiceHelper<T>.Service = service;
                return;
            }

            Debug.LogWarning($"ServiceLocator: service <{nameof(T)}> is already registered");
        }

        public static void Unregister<T>(T service = null) where T : class, IService
        {
            if (ServiceHelper<T>.Service != null) {
                ServiceHelper<T>.Service = null;
                return;
            }

            Debug.LogWarning($"ServiceLocator: service <{nameof(T)}> is not registered yet");
        }

        public static bool TryRegister<T>(T service) where T : class, IService
        {
            if (ServiceHelper<T>.Service == null) {
                ServiceHelper<T>.Service = service;
                return true;
            }

            return false;
        }

        public static T Resolve<T>() where T : class, IService
        {
            return ServiceHelper<T>.Service;
        }

        public static bool Has<T>() where T : class, IService
        {
            return ServiceHelper<T>.Service != null;
        }

        private static class ServiceHelper<T> where T : class, IService
        {
            public static T Service { get; set; } = null;
        }
    }
}

