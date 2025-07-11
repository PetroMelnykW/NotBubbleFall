using NotBubbleFall.Signals;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace NotBubbleFall.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScoreLabel : MonoBehaviour
    {
        private Tween _tween;
        private float _displayedScore;

        private TMP_Text _scoreLabel;

        private void Awake()
        {
            SignalBus.Subscribe<ScoreUpdatedSignal>(OnScoreUpdated);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<ScoreUpdatedSignal>(OnScoreUpdated);
        }

        private void Start()
        {
            _scoreLabel = GetComponent<TMP_Text>();
        }

        private void OnScoreUpdated(object sender, ScoreUpdatedSignal signalData)
        {
            _tween?.Kill();

            float targetScore = signalData.score;

            _tween = DOTween.To(
                () => _displayedScore,
                x => {
                    _displayedScore = x;
                    _scoreLabel.text = ((int)_displayedScore).ToString();
                },
                targetScore,
                0.5f
            ).SetEase(Ease.OutCubic);
        }
    }
}

