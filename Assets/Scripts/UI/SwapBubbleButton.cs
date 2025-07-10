using NotBubbleFall.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace NotBubbleFall.UI
{
    [RequireComponent(typeof(Button))]
    public class SwapBubbleButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SignalBus.Emit(this, new ProjectileSwapButtonPressedSignal { });
            });
        }
    }
}