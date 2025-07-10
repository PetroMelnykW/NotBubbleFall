namespace NotBubbleFall
{
    public interface IInitializableService : IService
    {
        void Resolve();
        void Initialize();
    }
}