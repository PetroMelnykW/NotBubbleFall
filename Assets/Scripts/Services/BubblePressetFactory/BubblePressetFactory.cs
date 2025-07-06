using NotBubbleFall.Data;
using NotBubbleFall.Gameplay;
using UnityEngine;

namespace NotBubbleFall.Services
{
    public class BubblePressetFactory : IBubblePressetFactory
    {
        private BubblePresetDB _bubblePresetDB;

        public GameObject CreateRandomBubblePresset(int phase)
        {
            var bubblePressetObject = Object.Instantiate(_bubblePresetDB.GetRandomPresset(phase));
            bubblePressetObject.GetComponent<BubblePresset>().Initialize();

            return bubblePressetObject;
        }

        public void Inject()
        {
            _bubblePresetDB = ServiceLocator.Resolve<BubblePresetDB>();
        }
    }

}

