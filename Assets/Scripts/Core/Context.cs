using UnityEngine;

namespace NotBubbleFall
{
    public abstract class Context : MonoBehaviour
    {
        private void Awake()
        {
            Create();
        }

        private void OnEnable()
        {
            Inject();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Delete();
        }

        protected virtual void Create() { }
        protected virtual void Inject() { }
        protected virtual void Initialize() { }
        protected virtual void Delete() { }
    }
}


