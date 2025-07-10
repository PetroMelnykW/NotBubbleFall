using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    abstract public class Projectile : MonoBehaviour
    {
        private const float DefaultSpeed = 10f;

        private float _speed = DefaultSpeed;

        private Rigidbody _rigidbody;

        public void SetDirection(Vector3 direction)
        {
            _rigidbody.linearVelocity = direction.normalized * _speed;
        }

        public void SetCustomSpeed(float speed)
        {
            _speed = speed;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (transform.position.z > 20)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Bubble bubble))
            {
                OnHitBubble(bubble);
            }
        }

        abstract protected void OnHitBubble(Bubble hitBubble);
    }
}

