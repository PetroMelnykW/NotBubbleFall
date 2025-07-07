using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace NotBubbleFall.Gameplay
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject _testProjectilePrefab;

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                var testProjectile = Instantiate(_testProjectilePrefab);
                testProjectile.transform.position = transform.position;
                testProjectile.GetComponent<Projectile>().SetDirection(Vector3.forward + Vector3.left);
            }
        }
    }
}


