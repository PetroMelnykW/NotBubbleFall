using NotBubbleFall;
using NotBubbleFall.Signals;
using UnityEngine;

namespace Services.Services
{
    public class ScoreManager : IScoreManager
    {
        public int RecordScore { get; private set; }
        public int Score { get; private set; }

        public void AddScore(int score)
        {
            Score += score;
            SignalBus.Emit(this, new ScoreUpdatedSignal { score = Score });
            if (Score > RecordScore)
            {
                RecordScore = Score;
                PlayerPrefs.SetInt("RecordScore", RecordScore);
                SignalBus.Emit(this, new RecordScoreUpdatedSignal { score = RecordScore });
            }
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public void Resolve() { }   

        public void Initialize()
        {
            RecordScore = PlayerPrefs.GetInt("RecordScore", 0);
        }
    }
}


