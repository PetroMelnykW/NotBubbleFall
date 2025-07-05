namespace NotBubbleFall
{
    public interface IInitializableService : IService
    {
        void Inject() { }
        void Initialize() { }
    }
}