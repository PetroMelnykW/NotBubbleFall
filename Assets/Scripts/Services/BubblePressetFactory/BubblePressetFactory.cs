using NotBubbleFall.Data;
using UnityEngine;

namespace NotBubbleFall.Services
{
    public class BubblePressetFactory : IBubblePressetFactory
    {
        private BubblePresetDB _bubblePresetDB;

        public GameObject CreateRandomBubblePresset(int phase)
        {
            return Object.Instantiate(_bubblePresetDB.GetRandomPresset(phase));
        }

        public void Inject()
        {
            _bubblePresetDB = ServiceLocator.Resolve<BubblePresetDB>();
        }
    }

}

