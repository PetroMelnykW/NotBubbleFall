using UnityEngine;

namespace NotBubbleFall.Signals
{
    public struct LaunchTouchMovedSignal : ISignal { public bool isInLaunchableZone; public Vector2 touchPosition; }
    public struct LaunchTouchEndedSignal : ISignal { public bool isInLaunchableZone; public Vector2 releasePosition; }
    public struct ProjectileSwapButtonPressedSignal : ISignal { }
}