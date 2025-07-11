namespace NotBubbleFall.Signals
{
    public struct ScoreUpdatedSignal : ISignal { public int score; }
    public struct RecordScoreUpdatedSignal : ISignal { public int score; }
}