using DG.Tweening;
using UnityEngine;

namespace NotBubbleFall.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EndMenu : MonoBehaviour, IAnimatable
    {
        public bool IsAnimating => _tween != null;
        private Tween _tween;
        private CanvasGroup _canvasGroup;
        public async Awaitable PlayAnimation(string animationName = "")
        {
            if (_tween != null) return;
            switch (animationName)
            {
                case "Show":
                    _canvasGroup.alpha = 0f;
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = true;
                    _tween = _canvasGroup.DOFade(1f, 0.8f)
                        .SetEase(Ease.OutCubic)
                        .OnComplete(() => _canvasGroup.interactable = true);
                    break;
                case "Hide":
                    _canvasGroup.alpha = 1f;
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = true;
                    _tween = _canvasGroup.DOFade(0f, 0.8f)
                        .SetEase(Ease.OutCubic)
                        .OnComplete(() => _canvasGroup.blocksRaycasts = false);
                    break;
                default:
                    Debug.LogWarning($"Animation '{animationName}' not found in {nameof(EndMenu)}.");
                    return;
            }
            await _tween.AsyncWaitForCompletion();
            _tween = null;
        }
        public void StopAnimation()
        {
            _tween?.Kill();
            _tween = null;
        }
        private void Awake()
        {
            ServiceLocator.Register(this);
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        private void OnDestroy()
        {
            ServiceLocator.Unregister(this);
            _tween?.Kill();
        }
    }
}

