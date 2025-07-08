using System;
using UnityEngine;
using NotBubbleFall.Gameplay;
using System.Linq;

namespace NotBubbleFall.Data
{
    [CreateAssetMenu(fileName = "BubbleData", menuName = "Data/ProjectileDB", order = 1)]
    public class ProjectileDB : ScriptableObject, IService
    {
        [SerializeField] private GameObject _bubbleProjectilePrefab;
        [SerializeField] private BubbleData[] _bubblesData;

        public GameObject BubbleProjectilePrefab => _bubbleProjectilePrefab;
        public BubbleData[] BubblesData => _bubblesData;

        public BubbleData GetBubbleData(BubbleColorType bubbleColor)
        {
            return _bubblesData.First(bubbleData => bubbleData.BubbleColor == bubbleColor);
        }
    }

    [Serializable]
    public class BubbleData
    {
        [SerializeField] private BubbleColorType _bubbleColor;
        [SerializeField] private Material _bubbleMaterial;

        public BubbleColorType BubbleColor => _bubbleColor;
        public Material BubbleMaterial => _bubbleMaterial;
    }
}