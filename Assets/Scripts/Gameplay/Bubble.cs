using DG.Tweening;
using NotBubbleFall.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public enum BubbleColorType
    {
        Default,
        Red,
        Blue,
        Green,
        Yellow,
        Purple
    }

    [RequireComponent(typeof(MeshRenderer))]
    public class Bubble : MonoBehaviour
    {
        public List<Bubble> Connections => _connections;
        public BubbleColorType BubbleColor => _bubbleColor;

        private List<Bubble> _connections = new List<Bubble>();
        private BubbleColorType _bubbleColor = BubbleColorType.Default;
        private Sequence _popSequence;

        private MeshRenderer _meshRenderer;
        private BubbleDB _bubbleDB;

        public void SetColor(BubbleColorType bubbleColor)
        {
            _bubbleColor = bubbleColor;
            _meshRenderer.material = _bubbleDB.GetBubbleData(bubbleColor).BubbleMaterial;
        }

        public void AddMutualConnection(Bubble bubble)
        {
            AddConnection(bubble);
            bubble.AddConnection(this);
        }

        public void RemoveMutualConnection(Bubble bubble)
        {
            RemoveConnection(bubble);
            bubble.RemoveConnection(this);
        }

        public void AddConnection(Bubble bubble)
        {
            if (!_connections.Contains(bubble))
            {
                _connections.Add(bubble);
            }
        }

        public void RemoveConnection(Bubble bubble)
        {
            if (_connections.Contains(bubble))
            {
                _connections.Remove(bubble);
            }
        }

        public void ClearConnections()
        {
            foreach (var connection in new List<Bubble>(_connections))
            {
                RemoveMutualConnection(connection);
            }
        }

        public void Pop()
        {
            Destroy(GetComponent<BubbleProjectile>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());

            _popSequence = DOTween.Sequence()
                .Append(transform.DOMoveY(transform.position.y + 1, 1f))
                .Join(transform.DOScale(0f, 1f))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 1f);
                });
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _bubbleDB = ServiceLocator.Resolve<BubbleDB>();

            // Set the initial bubble color based on the material
            _bubbleColor = _bubbleDB.BubblesData.FirstOrDefault(d => d.BubbleMaterial == _meshRenderer.sharedMaterial)?.BubbleColor ?? default;
        }

        private void OnDestroy()
        {
            _popSequence?.Kill();
        }
    }
}