using UnityEngine;

namespace NotBubbleFall
{
    public interface IBubblePressetFactory : IInitializableService
    {
        GameObject CreateRandomBubblePresset(int phase);
    }
}