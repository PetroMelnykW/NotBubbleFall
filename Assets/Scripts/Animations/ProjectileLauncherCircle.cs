using DG.Tweening;
using UnityEngine;

namespace NotBubbleFall.Animations
{
    public class ProjectileLauncherCircle : MonoBehaviour
    {
        const float OneCircleDuration = 3f;

        private void Start()
        {
            transform.DOLocalRotate(
            new Vector3(0f, 0f, 360f),
            OneCircleDuration,
            RotateMode.FastBeyond360
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart);
        }
    }
}