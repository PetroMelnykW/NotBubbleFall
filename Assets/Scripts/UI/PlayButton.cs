using UnityEngine;
using UnityEngine.UI;
using NotBubbleFall.Signals;

namespace NotBubbleFall.UI
{
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                SignalBus.Emit(this, new PlayButtonPressedSignal { });
            });
        }
    }
}

