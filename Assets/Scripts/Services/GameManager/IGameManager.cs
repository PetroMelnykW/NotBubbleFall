namespace NotBubbleFall
{
    public interface IGameManager : IService
    {
        bool IsGameRunning { get; }
        void StartGame();
        void EndGame();
        void RestartGame();
        void StopGame();
    }
}