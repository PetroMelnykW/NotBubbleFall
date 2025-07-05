using NotBubbleFall.Services;

namespace NotBubbleFall
{
    public class MainGameContext : Context
    {
        protected override void Create()
        {
            ServiceLocator.Register<IBubblePressetFactory>(new BubblePressetFactory());
        }

        protected override void Inject()
        {
            ServiceLocator.Resolve<IBubblePressetFactory>().Inject();
        }

        protected override void Initialize()
        {
            ServiceLocator.Resolve<IBubblePressetFactory>().Initialize();
        }

        protected override void Deinitialize()
        {
            ServiceLocator.Unregister<IBubblePressetFactory>();
        }
    }
}
