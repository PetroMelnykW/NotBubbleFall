using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    [RequireComponent(typeof(Bubble))]
    public class BubbleProjectile : Projectile
    {
        private static readonly Vector3[] Directions = new Vector3[]
        {
            new Vector3(0.3f, 0, 0),
            new Vector3(-0.3f, 0, 0),
            new Vector3(0.15f, 0, 0.25f),
            new Vector3(-0.15f, 0, 0.25f),
            new Vector3(0.15f, 0, -0.25f),
            new Vector3(-0.15f, 0, -0.25f),
        };

        private bool _isPlaced = false;

        protected override void OnHitBubble(Bubble bubble)
        {
            if (!_isPlaced)
            {
                PlaceBubbleOnField(bubble);
            }
        }

        private void PlaceBubbleOnField(Bubble hitBubble)
        {
            _isPlaced = true;

            // Place the bubble at the closest free position around the hit bubble
            var closestFreePosition = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

            foreach (var direction in Directions)
            {
                var placementPosition = hitBubble.transform.position + direction;

                if (!Physics.CheckSphere(placementPosition, 0.14f, 1 << LayerMask.NameToLayer("Default")))
                {
                    if (Vector3.Distance(transform.position, placementPosition) < Vector3.Distance(transform.position, closestFreePosition))
                    {
                        closestFreePosition = placementPosition;
                    }
                }
            }

            transform.position = closestFreePosition;

            // Set connections with neighbouring bubbles
            var bubble = GetComponent<Bubble>();

            foreach (var hit in Physics.OverlapSphere(hitBubble.transform.position, 0.3f))
            {
                if (hit.TryGetComponent(out Bubble neighbourbubble))
                {
                    bubble.AddMutualConnection(neighbourbubble);
                }
            }

            var fieldController = ServiceLocator.Resolve<IFieldController>();
            fieldController.AddBubble(bubble);

            gameObject.layer = LayerMask.NameToLayer("Default");

            Destroy(this);
            Destroy(GetComponent<Rigidbody>());
        }
    }
}
