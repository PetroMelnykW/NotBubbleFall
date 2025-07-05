using NotBubbleFall.Gameplay;
using System.Collections.Generic;

namespace NotBubbleFall
{
    public interface IFieldController : IService
    {
        int CurrentPhase { get; }
        List<BubbleColorType> GetAllowedColors();
        void StardField();
        void StopField();
        void ResetField();
    }
}