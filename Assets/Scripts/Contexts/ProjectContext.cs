using NotBubbleFall.Data;
using Services.Services;
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
            ServiceLocator.Register<IScoreManager>(new ScoreManager());
        }

        protected override void Inject()
        {
            ServiceLocator.Resolve<IScoreManager>().Resolve();
        }

        protected override void Initialize()
        {
            ServiceLocator.Resolve<IScoreManager>().Initialize();
        }

        protected override void Delete()
        {
            ServiceLocator.Unregister(_projectileDB);
            ServiceLocator.Unregister(_bubbleDB);
            ServiceLocator.Unregister<IScoreManager>();
        }
    }
}


