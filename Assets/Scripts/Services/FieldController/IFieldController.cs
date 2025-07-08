using NotBubbleFall.Gameplay;

namespace NotBubbleFall
{
    public interface IFieldController : IService
    {
        int CurrentPhase { get; }
        void StardField();
        void StopField();
        void ResetField();
        void AddBubble(Bubble bubble);
        void RemoveBubble(Bubble bubble);
    }
}