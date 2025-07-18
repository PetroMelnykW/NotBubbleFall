using UnityEngine;
using NotBubbleFall.Signals;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace NotBubbleFall.UI
{
    public class LaunchDetectionArea : MonoBehaviour
    {
        private int _activeTouchId = -1;

        private RectTransform _rectTransform;
        private IGameManager _gameManager;

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            TouchSimulation.Disable();
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _gameManager = ServiceLocator.Resolve<IGameManager>();
        }

        private void LateUpdate()
        {
            HandleTouch();
        }

        private void HandleTouch()
        {
            if (!_gameManager.IsGameRunning)
            {
                _activeTouchId = -1;
                return;
            }

            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == TouchPhase.Began && _activeTouchId == -1)
                {
                    _activeTouchId = touch.touchId;
                    var touchPosition = touch.screenPosition;
                    var isInLaunchableZone = IsPointerInsideRect(touchPosition);
                    SignalBus.Emit(this, new LaunchTouchMovedSignal { isInLaunchableZone = isInLaunchableZone, touchPosition = touchPosition });
                }
                else if (touch.phase == TouchPhase.Moved && _activeTouchId == touch.touchId)
                {
                    var touchPosition = touch.screenPosition;
                    var isInLaunchableZone = IsPointerInsideRect(touchPosition);
                    SignalBus.Emit(this, new LaunchTouchMovedSignal { isInLaunchableZone = isInLaunchableZone, touchPosition = touchPosition });
                }
                else if (touch.phase == TouchPhase.Ended && _activeTouchId == touch.touchId)
                {
                    var touchPosition = touch.screenPosition;
                    var isInLaunchableZone = IsPointerInsideRect(touchPosition);
                    SignalBus.Emit(this, new LaunchTouchEndedSignal { isInLaunchableZone = isInLaunchableZone, releasePosition = touchPosition });
                    _activeTouchId = -1;
                }
            }
        }

        private bool IsPointerInsideRect(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, screenPoint, null);
        }
    }
}