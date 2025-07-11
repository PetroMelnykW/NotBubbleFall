using NotBubbleFall.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace NotBubbleFall.UI
{
    [RequireComponent(typeof(Button))]
    public class AgainButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                SignalBus.Emit(this, new AgainButtonPressedSignal { });
            });
        }
    }
}


