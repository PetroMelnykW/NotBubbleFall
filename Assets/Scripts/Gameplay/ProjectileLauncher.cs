using NotBubbleFall.Data;
using NotBubbleFall.Signals;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject _testProjectilePrefab;

        private BubbleProjectile _currentProjectile;
        private Projectile _replacingProjectile = null;

        private ProjectileDB _projectileDB;

        private void Awake()
        {
            SignalBus.Subscribe<LaunchTouchMovedSignal>(OnLaunchTouchMoved);
            SignalBus.Subscribe<LaunchTouchEndedSignal>(OnLaunchTouchEnded);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<LaunchTouchMovedSignal>(OnLaunchTouchMoved);
            SignalBus.Unsubscribe<LaunchTouchEndedSignal>(OnLaunchTouchEnded);
        }

        private void Start()
        {
            _projectileDB = ServiceLocator.Resolve<ProjectileDB>();
        }

        private void LaunchProjectile(Vector3 direction)
        {
            // TODO
            var projectile = Instantiate(_testProjectilePrefab, transform.position, Quaternion.identity).GetComponent<BubbleProjectile>();
            projectile.SetDirection(direction);
        }

        private void SwitchBubbles()
        {

        }

        private Vector3 GetPointAtHeight(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            float t = (0.25f - ray.origin.y) / ray.direction.y;

            return ray.origin + ray.direction * t;
        }

        private void OnLaunchTouchMoved(object sender, LaunchTouchMovedSignal signalData)
        {
            
        }

        private void OnLaunchTouchEnded(object sender, LaunchTouchEndedSignal signalData)
        {
            if (signalData.isInLaunchableZone)
            {
                LaunchProjectile((GetPointAtHeight(signalData.releasePosition) - transform.position).normalized);
            }
        }
    }
}


