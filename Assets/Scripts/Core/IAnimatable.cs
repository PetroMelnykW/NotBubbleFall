using UnityEngine;

namespace NotBubbleFall
{
    public interface IAnimatable : IService
    {
        bool IsAnimating { get; }
        Awaitable PlayAnimation(string animationName = "");
        void StopAnimation();
    }
}

