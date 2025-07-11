using DG.Tweening;
using UnityEngine;

namespace NotBubbleFall.Project
{
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            DOTween.SetTweensCapacity(1000, 500);
        }
    }
}

