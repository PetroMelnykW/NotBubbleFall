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
    public class Bubble : Projectile
    {
        public List<Bubble> Connections => _connections;
        public BubbleColorType BubbleColor => _bubbleColor;

        private List<Bubble> _connections = new List<Bubble>();
        private BubbleColorType _bubbleColor = BubbleColorType.Default;

        private MeshRenderer _meshRenderer;
        private ProjectileDB _projectileDB;

        public void Initialize()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _projectileDB = ServiceLocator.Resolve<ProjectileDB>();

            // Set the initial bubble color based on the material
            _bubbleColor = _projectileDB.BubblesData.FirstOrDefault(d => d.BubbleMaterial == _meshRenderer.sharedMaterial)?.BubbleColor ?? default;
        }

        public void SetColor(BubbleColorType bubbleColor)
        {
            _bubbleColor = bubbleColor;
            _meshRenderer.material = _projectileDB.GetBubbleData(bubbleColor).BubbleMaterial;
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
    }
}