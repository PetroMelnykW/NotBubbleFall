using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    [RequireComponent(typeof(Bubble))]
    public class BubbleProjectile : Projectile
    {
        private bool _hitted = false;

        protected override void OnHitBubble(Bubble hitBubble)
        {
            if (!_hitted)
            {
                _hitted = true;

                var fieldController = ServiceLocator.Resolve<IFieldController>();
                fieldController.AttachBubble(GetComponent<Bubble>(), hitBubble);

                var bubbleRigidbody = GetComponent<Rigidbody>();
                bubbleRigidbody.isKinematic = true;

                Destroy(this);
            }
        }
    }
}
