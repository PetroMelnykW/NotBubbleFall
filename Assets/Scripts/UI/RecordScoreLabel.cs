using NotBubbleFall.Signals;
using TMPro;
using UnityEngine;

namespace NotBubbleFall.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class RecordScoreLabel : MonoBehaviour
    {
        private TMP_Text _recordScoreLabel;

        private void Awake()
        {
            SignalBus.Subscribe<RecordScoreUpdatedSignal>(OnRecordScoreUpdated);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<RecordScoreUpdatedSignal>(OnRecordScoreUpdated);
        }

        private void Start()
        {
            _recordScoreLabel = GetComponent<TMP_Text>();
            _recordScoreLabel.text = ServiceLocator.Resolve<IScoreManager>().RecordScore.ToString();
        }

        private void OnRecordScoreUpdated(object sender, RecordScoreUpdatedSignal signalData)
        {
            _recordScoreLabel.text = signalData.score.ToString();
        }
    }
}


