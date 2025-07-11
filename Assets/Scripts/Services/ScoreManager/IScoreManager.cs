namespace NotBubbleFall
{
    public interface IScoreManager : IInitializableService
    {
        int RecordScore { get; }
        int Score { get; }
        void AddScore(int score);
        void ResetScore();
    }
}


