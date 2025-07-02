using UnityEngine;

namespace NotBubbleFall
{
    public abstract class Context : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Deinitialize();
        }

        protected abstract void Initialize();

        protected abstract void Deinitialize();
    }
}


