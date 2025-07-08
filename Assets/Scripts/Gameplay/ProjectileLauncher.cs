using NotBubbleFall.Data;
using NotBubbleFall.Signals;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public class ProjectileLauncher : MonoBehaviour
    {
        const float HintMarkerShowLimit = 1.8f;

        [SerializeField] private Transform _directionHint;

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
            var projectile = Instantiate(_projectileDB.BubbleProjectilePrefab, transform.position, Quaternion.identity).GetComponent<BubbleProjectile>();
            projectile.SetDirection(direction);
        }

        private void SetHintDirection(Vector3 target)
        {
            var targetPosition = new Vector3(target.x, 0.25f, target.z);
            _directionHint.transform.LookAt(targetPosition);
            foreach (Transform marker in _directionHint)
            {
                marker.gameObject.SetActive(Mathf.Abs(marker.position.x) < HintMarkerShowLimit);
            }
        }

        private void SwitchBubbles()
        {

        }

        private Vector3 GetPointInWorld(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            float t = (0.25f - ray.origin.y) / ray.direction.y;

            return ray.origin + ray.direction * t;
        }

        private void OnLaunchTouchMoved(object sender, LaunchTouchMovedSignal signalData)
        {
            _directionHint.gameObject.SetActive(signalData.isInLaunchableZone);
            if (signalData.isInLaunchableZone)
            {
                SetHintDirection(GetPointInWorld(signalData.touchPosition));
            }
        }

        private void OnLaunchTouchEnded(object sender, LaunchTouchEndedSignal signalData)
        {
            if (signalData.isInLaunchableZone)
            {
                LaunchProjectile((GetPointInWorld(signalData.releasePosition) - transform.position).normalized);
                _directionHint.gameObject.SetActive(false);
            }
        }
    }
}


