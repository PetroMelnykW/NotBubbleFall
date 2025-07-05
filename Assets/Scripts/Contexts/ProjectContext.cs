using NotBubbleFall.Data;
using UnityEngine;

namespace NotBubbleFall
{
    public class ProjectContext : Context
    {
        [SerializeField] private ProjectileDB _projectileDB;
        [SerializeField] private BubblePresetDB _bubblePresetDB;

        protected override void Create()
        {
            ServiceLocator.Register(_projectileDB);
            ServiceLocator.Register(_bubblePresetDB);
        }

        protected override void Deinitialize()
        {
            ServiceLocator.Unregister(_projectileDB);
            ServiceLocator.Unregister(_bubblePresetDB);
        }
    }
}


