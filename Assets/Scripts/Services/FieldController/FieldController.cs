using NotBubbleFall.Data;
using NotBubbleFall.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBubbleFall.Services
{
    public class FieldController : MonoBehaviour, IFieldController
    {
        const float BubbleRowInterval = 0.25f;

        [SerializeField] private float _fieldSpeed = 1.0f;

        [Space(10)]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _gameEndPoint;
        [SerializeField] private Transform _bubbleParent;

        public int CurrentPhase => _currentPhase;

        private int _currentPhase = 0;
        private bool _isFieldActive = false;
        private List<Bubble> _bubbles = new List<Bubble>();
        private Bubble[] _rootBubbles = new Bubble[13];

        private ProjectileLauncher _projectileLauncher;
        private ProjectileDB _projectileDB;
        private IBubblePressetFactory _bubblePressetFactory;
        private IGameManager _gameManager;

        public void StardField()
        {
            _projectileLauncher.LoadLauncher();
            _isFieldActive = true;
        }

        public void StopField()
        {
            _projectileLauncher.UnloadLauncher();
            _isFieldActive = false;
        }

        public void ResetField()
        {
            _projectileLauncher.UnloadLauncher();
            foreach (var bubble in _bubbles)
            {
                Destroy(bubble.gameObject);
            }
            _projectileLauncher.LoadLauncher();
        }

        public void AddBubble(Bubble bubble)
        {
            if (!_bubbles.Contains(bubble))
            {
                _bubbles.Add(bubble);
                bubble.transform.SetParent(_bubbleParent, true);
            }
        }

        public void RemoveBubble(Bubble bubble)
        {
            if (_bubbles.Contains(bubble))
            {
                _bubbles.Remove(bubble);
            }
        }

        private void Awake()
        {
            ServiceLocator.Register<IFieldController>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IFieldController>(this);
        }

        private void Start()
        {
            _projectileLauncher = ServiceLocator.Resolve<ProjectileLauncher>();
            _projectileDB = ServiceLocator.Resolve<ProjectileDB>();
            _bubblePressetFactory = ServiceLocator.Resolve<IBubblePressetFactory>();
            _gameManager = ServiceLocator.Resolve<IGameManager>();
        }

        private void FixedUpdate()
        {
            if (_isFieldActive)
            {
                ProcessFieldMovement();
                CheckSpawnPoint();
            }
        }

        private void ProcessFieldMovement()
        {
            foreach (var bubble in _bubbles)
            {
                bubble.transform.position += Vector3.back * _fieldSpeed * Time.fixedDeltaTime;

                if (bubble.transform.position.z < _gameEndPoint.position.z)
                {
                    _gameManager.EndGame();
                }
            }
        }

        private void CheckSpawnPoint()
        {
            if (IsRootRowEmpty() || _rootBubbles[0].transform.position.z < _spawnPoint.position.z)
            {
                SpawnBubblePresset();
            }
        }

        private void SpawnBubblePresset()
        {
            GameObject bubblePressetObject = _bubblePressetFactory.CreateRandomBubblePresset(CurrentPhase);
            Transform pressetTransform = bubblePressetObject.transform;

            // Set initial position of the presset
            if (IsRootRowEmpty())
            {
                pressetTransform.position = _spawnPoint.position;
            }
            else
            {
                var rootBubbleZPosition = _rootBubbles.First(b => b != null).GetComponent<Transform>().position.z;

                pressetTransform.position = new Vector3(0, _spawnPoint.position.y, rootBubbleZPosition + BubbleRowInterval);
            }

            // Set connections between last root bubbles and the first row of the presset
            for (int i = 0; i < 12; i++)
            {
                var bubble = pressetTransform.GetChild(0).GetChild(i).GetComponent<Bubble>();

                if (_rootBubbles[i])
                {
                    bubble.AddMutualConnection(_rootBubbles[i]);
                }

                if (_rootBubbles[i + 1])
                {
                    bubble.AddMutualConnection(_rootBubbles[i + 1]);
                }
            }

            // Set root bubbles from the new presset
            for (int i = 0; i < 13; i++)
            {
                _rootBubbles[i] = pressetTransform.GetChild(pressetTransform.childCount - 1).GetChild(i).GetComponent<Bubble>();
            }

            // Set connections between bubbles in the presset
            for (int rowIndex = 0; rowIndex < pressetTransform.childCount - 1; rowIndex++)
            {
                var offset = rowIndex % 2 == 0 ? 0 : 1;

                for (int bubbleIndex = 0; bubbleIndex < 12; bubbleIndex++)
                {
                    var bubble = pressetTransform.GetChild(rowIndex + offset).GetChild(bubbleIndex).GetComponent<Bubble>();

                    bubble.AddConnection(pressetTransform.GetChild(rowIndex + 1 - offset).GetChild(bubbleIndex).GetComponent<Bubble>());
                    bubble.AddConnection(pressetTransform.GetChild(rowIndex + 1 - offset).GetChild(bubbleIndex + 1).GetComponent<Bubble>());
                }
            }

            // Reparent bubbles from the presset to the bubble parent
            var pressetBubbles = new List<Bubble>();

            for (int rowIndex = 0; rowIndex < pressetTransform.childCount; rowIndex++)
            {
                for (int i = 0; i < pressetTransform.GetChild(rowIndex).childCount; i++)
                {
                    pressetBubbles.Add(pressetTransform.GetChild(rowIndex).GetChild(i).GetComponent<Bubble>());
                }
            }

            foreach (var bubble in pressetBubbles)
            {
                bubble.transform.SetParent(_bubbleParent, true);
            }

            _bubbles.AddRange(pressetBubbles);

            Destroy(bubblePressetObject);
        }

        private bool IsRootRowEmpty()
        {
            return _rootBubbles.All(b => b == null);
        }
    }
}
