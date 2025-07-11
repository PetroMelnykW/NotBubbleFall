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
        const int BubbleScoreValue = 10;

        private static readonly Vector3[] HexNeighborOffsets = new Vector3[]
        {
            new Vector3(0.3f, 0, 0),
            new Vector3(-0.3f, 0, 0),
            new Vector3(0.15f, 0, 0.25f),
            new Vector3(-0.15f, 0, 0.25f),
            new Vector3(0.15f, 0, -0.25f),
            new Vector3(-0.15f, 0, -0.25f),
        };

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
        private IScoreManager _scoreManager;

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

        public void AttachBubble(Bubble newBubble, Bubble anchorBubble)
        {
            // Find the closest free position around the anchor bubble
            var closestFreePosition = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

            foreach (var hexNeighborOffset in HexNeighborOffsets)
            {
                var placementPosition = anchorBubble.transform.position + hexNeighborOffset;

                var isPlaceFree = !Physics.CheckSphere(placementPosition, 0.14f, 1 << LayerMask.NameToLayer("Default"));
                var isPlaceCloser = Vector3.Distance(newBubble.transform.position, placementPosition) < Vector3.Distance(newBubble.transform.position, closestFreePosition);

                if (isPlaceFree && isPlaceCloser)
                {
                    closestFreePosition = placementPosition;
                }
            }

            newBubble.transform.position = closestFreePosition;

            // Set the new bubble's connections
            foreach (var hit in Physics.OverlapSphere(newBubble.transform.position, 0.3f))
            {
                if (hit.TryGetComponent(out Bubble neighbourbubble))
                {
                    newBubble.AddMutualConnection(neighbourbubble);
                }
            }

            AddBuble(newBubble);
            FindAndDestroySameColorBubbles(newBubble);
            RemoveDisconnectedBubbles();
        }

        public void PopBubble(Bubble bubble)
        {
            RemoveBubble(bubble);
            RemoveDisconnectedBubbles();
        }

        public void PopBubbles(IEnumerable<Bubble> bubbles)
        {
            foreach (var bubble in bubbles)
            {
                RemoveBubble(bubble);
            }
            RemoveDisconnectedBubbles();
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
            _scoreManager = ServiceLocator.Resolve<IScoreManager>();
        }

        private void FixedUpdate()
        {
            if (_isFieldActive)
            {
                ProcessFieldMovement();
                CheckSpawnPoint();
            }
        }

        private void AddBuble(Bubble bubble)
        {
            _bubbles.Add(bubble);
            bubble.gameObject.layer = LayerMask.NameToLayer("Default");
            bubble.transform.SetParent(_bubbleParent, true);
        }

        private void RemoveBubble(Bubble bubble)
        {
            _bubbles.Remove(bubble);
            if (_rootBubbles.Contains(bubble))
            {
                int index = System.Array.IndexOf(_rootBubbles, bubble);
                _rootBubbles[index] = null;
            }
            bubble.ClearConnections();
            _scoreManager.AddScore(BubbleScoreValue);
            bubble.Pop();
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

        private void FindAndDestroySameColorBubbles(Bubble startBubble)
        {
            var targetColor = startBubble.BubbleColor;

            HashSet<Bubble> matchingBubbles = new HashSet<Bubble>();
            Stack<Bubble> stack = new Stack<Bubble>();

            stack.Push(startBubble);
            matchingBubbles.Add(startBubble);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                foreach (var neighbor in current.Connections)
                {
                    if (neighbor == null || matchingBubbles.Contains(neighbor))
                        continue;

                    if (neighbor.BubbleColor == targetColor)
                    {
                        matchingBubbles.Add(neighbor);
                        stack.Push(neighbor);
                    }
                }
            }

            if (matchingBubbles.Count >= 3)
            {
                foreach (var bubble in matchingBubbles)
                {
                    RemoveBubble(bubble);
                }
            }
        }

        private void RemoveDisconnectedBubbles()
        {
            HashSet<Bubble> connectedToRoots = new HashSet<Bubble>();
            Stack<Bubble> stack = new Stack<Bubble>();

            foreach (var root in _rootBubbles)
            {
                if (root == null || connectedToRoots.Contains(root))
                    continue;

                stack.Push(root);
                connectedToRoots.Add(root);

                while (stack.Count > 0)
                {
                    var current = stack.Pop();

                    foreach (var neighbor in current.Connections)
                    {
                        if (neighbor == null || connectedToRoots.Contains(neighbor))
                            continue;

                        connectedToRoots.Add(neighbor);
                        stack.Push(neighbor);
                    }
                }
            }

            List<Bubble> disconnectedBubbles = new List<Bubble>();

            foreach (var bubble in _bubbles)
            {
                if (!connectedToRoots.Contains(bubble))
                {
                    disconnectedBubbles.Add(bubble);
                }
            }

            foreach (var bubble in disconnectedBubbles)
            {
                RemoveBubble(bubble);
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

                // Set connections between bubbles in the same row
                for (int bubbleIndex = 0; bubbleIndex < 12 + offset - 1; bubbleIndex++)
                {
                    var bubble = pressetTransform.GetChild(rowIndex).GetChild(bubbleIndex).GetComponent<Bubble>();
                    var nextBubbleIndex = pressetTransform.GetChild(rowIndex).GetChild(bubbleIndex + 1).GetComponent<Bubble>();

                    bubble.AddMutualConnection(nextBubbleIndex);
                }

                // Set connections between bubbles in the neighboring rows
                for (int bubbleIndex = 0; bubbleIndex < 12; bubbleIndex++)
                {
                    var bubble = pressetTransform.GetChild(rowIndex + offset).GetChild(bubbleIndex).GetComponent<Bubble>();

                    bubble.AddMutualConnection(pressetTransform.GetChild(rowIndex + 1 - offset).GetChild(bubbleIndex).GetComponent<Bubble>());
                    bubble.AddMutualConnection(pressetTransform.GetChild(rowIndex + 1 - offset).GetChild(bubbleIndex + 1).GetComponent<Bubble>());
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
