using NotBubbleFall.Data;
using UnityEngine;

namespace NotBubbleFall
{
    public class ProjectContext : Context
    {
        [SerializeField] private ProjectileDB _projectileDB;
        [SerializeField] private BubbleDB _bubbleDB;

        protected override void Create()
        {
            ServiceLocator.Register(_projectileDB);
            ServiceLocator.Register(_bubbleDB);
        }

        protected override void Delete()
        {
            ServiceLocator.Unregister(_projectileDB);
            ServiceLocator.Unregister(_bubbleDB);
        }
    }
}


