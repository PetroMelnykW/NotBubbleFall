using DG.Tweening;
using NotBubbleFall.Signals;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public class ProjectileLauncher : MonoBehaviour, IService
    {
        private const float HintMarkerShowLimit = 1.8f;
        private const float NextProjectileScaleRate = 0.7f;
        private const float LaunchSpeed = 1.0f;
        private const float SwapSpeed = 0.8f;
        private const float ReloadSpeed = 0.5f;

        [SerializeField] private Transform _currentProjectilePoint;
        [SerializeField] private Transform _nextProjectilePoint;
        [SerializeField] private Transform _directionHint;

        private Projectile _currentBubbleProjectile;
        private Projectile _nextBubbleProjectile;
        private Tween _animationTween = null;

        private IProjectileFactory _projectileFactory;

        public void LoadLauncher()
        {
            _currentBubbleProjectile = _projectileFactory.CreateBubbleProjectile();
            _currentBubbleProjectile.transform.position = _currentProjectilePoint.position;

            _nextBubbleProjectile = _projectileFactory.CreateBubbleProjectile();
            _nextBubbleProjectile.transform.position = _nextProjectilePoint.position;

            PlayAnimation("LoadLauncher");
        }

        public void UnloadLauncher()
        {
            Destroy(_currentBubbleProjectile.gameObject);
            Destroy(_nextBubbleProjectile.gameObject);
            _animationTween.Kill();
            _animationTween = null;
        }

        private void Awake()
        {
            ServiceLocator.Register(this);
            SignalBus.Subscribe<LaunchTouchMovedSignal>(OnLaunchTouchMoved);
            SignalBus.Subscribe<LaunchTouchEndedSignal>(OnLaunchTouchEnded);
            SignalBus.Subscribe<ProjectileSwapButtonPressedSignal>(OnProjectileSwapButtonPressed);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister(this);
            SignalBus.Unsubscribe<LaunchTouchMovedSignal>(OnLaunchTouchMoved);
            SignalBus.Unsubscribe<LaunchTouchEndedSignal>(OnLaunchTouchEnded);
            SignalBus.Unsubscribe<ProjectileSwapButtonPressedSignal>(OnProjectileSwapButtonPressed);
        }

        private void Start()
        {
            _projectileFactory = ServiceLocator.Resolve<IProjectileFactory>();
        }

        private void LaunchProjectile(Vector3 direction)
        {
            _currentBubbleProjectile.SetDirection(direction);
            _currentBubbleProjectile = _nextBubbleProjectile;
            _nextBubbleProjectile = _projectileFactory.CreateRandomProjectile();
            _nextBubbleProjectile.transform.position = _nextProjectilePoint.position;

            PlayAnimation("LoadNextProjectile");
        }

        private void SwapBubbles()
        {
            var temp = _currentBubbleProjectile;
            _currentBubbleProjectile = _nextBubbleProjectile;
            _nextBubbleProjectile = temp;

            PlayAnimation("SwapProjectiles");
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

        private void PlayAnimation(string animationName)
        {
            _animationTween?.Kill();

            switch (animationName)
            {
                case "LoadLauncher":
                {
                    var currentBubbleTargetScale = _currentBubbleProjectile.transform.localScale;
                    var nextBubbleTargetScale = _nextBubbleProjectile.transform.localScale * NextProjectileScaleRate;

                    _currentBubbleProjectile.transform.localScale = Vector3.zero;
                    _nextBubbleProjectile.transform.localScale = Vector3.zero;

                    _animationTween = DOTween.Sequence()
                        .Append(_currentBubbleProjectile.transform.DOScale(currentBubbleTargetScale, 1f))
                        .Join(_nextBubbleProjectile.transform.DOScale(nextBubbleTargetScale, 1f));

                    break;
                }
                case "SwapProjectiles":
                {
                    var currentBubbleTargetScale = _nextBubbleProjectile.transform.localScale;
                    var nextBubbleTargetScale = _currentBubbleProjectile.transform.localScale;

                    _animationTween = DOTween.Sequence()
                        .Append(_currentBubbleProjectile.transform.DOScale(currentBubbleTargetScale, SwapSpeed))
                        .Join(_currentBubbleProjectile.transform.DOMove(_currentProjectilePoint.position, SwapSpeed))
                        .Join(_nextBubbleProjectile.transform.DOScale(nextBubbleTargetScale, SwapSpeed))
                        .Join(_nextBubbleProjectile.transform.DOMove(_nextProjectilePoint.position, SwapSpeed));

                    break;
                }
                case "LoadNextProjectile":
                {
                    var currentBubbleTargetScale = _currentBubbleProjectile.transform.localScale / NextProjectileScaleRate;
                    var nextBubbleTargetScale = _nextBubbleProjectile.transform.localScale * NextProjectileScaleRate;

                    _nextBubbleProjectile.transform.localScale = Vector3.zero;

                    _animationTween = DOTween.Sequence()
                        .Append(_currentBubbleProjectile.transform.DOScale(currentBubbleTargetScale, ReloadSpeed))
                        .Join(_currentBubbleProjectile.transform.DOMove(_currentProjectilePoint.position, ReloadSpeed))
                        .Join(_nextBubbleProjectile.transform.DOScale(nextBubbleTargetScale, ReloadSpeed));

                    break;
                }
            }

            _animationTween.OnComplete(() =>
            {
                _animationTween = null;
            });
        }

        private Vector3 GetPointInWorld(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            float t = (0.25f - ray.origin.y) / ray.direction.y;

            return ray.origin + ray.direction * t;
        }

        private void OnLaunchTouchMoved(object sender, LaunchTouchMovedSignal signalData)
        {
            var showHint = signalData.isInLaunchableZone && _animationTween == null;

            _directionHint.gameObject.SetActive(showHint);
            if (showHint)
            {
                SetHintDirection(GetPointInWorld(signalData.touchPosition));
            }
        }

        private void OnLaunchTouchEnded(object sender, LaunchTouchEndedSignal signalData)
        {
            var canLaunch = signalData.isInLaunchableZone && _animationTween == null && _directionHint.gameObject.activeSelf;

            if (canLaunch)
            {
                LaunchProjectile((GetPointInWorld(signalData.releasePosition) - transform.position).normalized);
                _directionHint.gameObject.SetActive(false);
            }
        }

        private void OnProjectileSwapButtonPressed(object sender, ProjectileSwapButtonPressedSignal signalData)
        {
            var canSwitch = _animationTween == null;

            if (canSwitch)
            {
                SwapBubbles();
                _directionHint.gameObject.SetActive(false);
            }
        }
    }
}


