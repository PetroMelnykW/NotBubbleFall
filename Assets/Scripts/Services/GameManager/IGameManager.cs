using UnityEngine;

namespace NotBubbleFall
{
    public interface IGameManager : IService
    {
        void StartGame();
        void EndGame();
        void ResetGame();
    }
}