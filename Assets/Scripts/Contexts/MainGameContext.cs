using NotBubbleFall.Services;

namespace NotBubbleFall
{
    public class MainGameContext : Context
    {
        protected override void Create()
        {
            ServiceLocator.Register<IBubblePressetFactory>(new BubblePressetFactory());
            ServiceLocator.Register<IProjectileFactory>(new ProjectileFactory());
        }

        protected override void Inject()
        {
            ServiceLocator.Resolve<IBubblePressetFactory>().Resolve();
            ServiceLocator.Resolve<IProjectileFactory>().Resolve();
        }

        protected override void Initialize()
        {
            ServiceLocator.Resolve<IBubblePressetFactory>().Initialize();
            ServiceLocator.Resolve<IProjectileFactory>().Initialize();
        }

        protected override void Delete()
        {
            ServiceLocator.Unregister<IBubblePressetFactory>();
            ServiceLocator.Unregister<IProjectileFactory>();
        }
    }
}
