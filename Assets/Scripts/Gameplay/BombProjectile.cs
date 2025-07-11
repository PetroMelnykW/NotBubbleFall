using System.Collections.Generic;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public class BombProjectile : Projectile
    {
        const float BombRadius = 2.0f;

        protected override void OnHitBubble(Bubble hitBubble)
        {
            var hitBubbles = new List<Bubble>();

            foreach (var hit in Physics.OverlapSphere(transform.position, BombRadius, 1 << LayerMask.NameToLayer("Default")))
            {
                Bubble bubble = hit.GetComponent<Bubble>();

                if (bubble != null)
                {
                    hitBubbles.Add(bubble);
                }
            }

            var fieldController = ServiceLocator.Resolve<IFieldController>();
            fieldController.PopBubbles(hitBubbles);

            Destroy(gameObject);
        }
    }
}

