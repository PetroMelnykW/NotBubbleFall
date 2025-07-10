using NotBubbleFall.Data;
using NotBubbleFall.Gameplay;
using UnityEngine;

namespace NotBubbleFall.Services
{
    public class BubblePressetFactory : IBubblePressetFactory
    {
        private BubbleDB _bubbleDB;

        public GameObject CreateRandomBubblePresset(int phase)
        {
            var bubblePressetObject = Object.Instantiate(_bubbleDB.GetRandomUnlcokedPressetForPhase(phase));
            bubblePressetObject.GetComponent<BubblePresset>().Initialize();

            return bubblePressetObject;
        }

        public void Resolve()
        {
            _bubbleDB = ServiceLocator.Resolve<BubbleDB>();
        }

        public void Initialize() { }
    }

}

