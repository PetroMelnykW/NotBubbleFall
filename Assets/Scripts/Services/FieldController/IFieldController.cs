using NotBubbleFall.Gameplay;
using System.Collections.Generic;

namespace NotBubbleFall
{
    public interface IFieldController : IService
    {
        int CurrentPhase { get; }
        void StardField();
        void StopField();
        void ClearField();
        void RestartField();
        void AttachBubble(Bubble newBubble, Bubble anchorBubble);
        void PopBubble(Bubble bubble);
        void PopBubbles(IEnumerable<Bubble> bubbles);
    }
}