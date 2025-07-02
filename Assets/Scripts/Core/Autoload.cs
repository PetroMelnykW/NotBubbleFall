using UnityEngine;

namespace NotBubbleFall.Core
{
    public static class Autoload
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoaded()
        {
            GameObject loadedResource = Resources.Load<GameObject>("Autoload");

            if (loadedResource != null) {
                GameObject autoload = Object.Instantiate(loadedResource);
                autoload.name = "Autoload";
                Object.DontDestroyOnLoad(autoload);
            }
            else {
                Debug.LogWarning("Autoload: Failded to load <Autoload> prefab");
            }
        }
    }
}

