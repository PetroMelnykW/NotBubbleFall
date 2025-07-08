using NotBubbleFall.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NotBubbleFall.Data
{
    [CreateAssetMenu(fileName = "BubbleDB", menuName = "Data/BubbleDB")]
    public class BubbleDB : ScriptableObject, IService
    {
        [SerializeField] private BubbleData[] _bubblesData;
        [SerializeField] private List<PhaseConfig> _phaseConfigs;

        public BubbleData[] BubblesData => _bubblesData;

        public BubbleData GetBubbleData(BubbleColorType bubbleColor)
        {
            return _bubblesData.First(bubbleData => bubbleData.BubbleColor == bubbleColor);
        }

        public GameObject GetRandomUnlcokedPressetForPhase(int phase)
        {
            List<GameObject> unlockedPressets = _phaseConfigs
                .Where(cfg => cfg.Phase <= phase)
                .SelectMany(cfg => cfg.UnlockedPresets)
                .Distinct()
                .ToList();

            if (unlockedPressets.Count == 0)
            {
                Debug.LogWarning($"No presets available for phase {phase}");
                return null;
            }

            return unlockedPressets[UnityEngine.Random.Range(0, unlockedPressets.Count)];
        }

        public List<BubbleColorType> GetUnlockedColorsForPhase(int phase)
        {
            List<BubbleColorType> unlockedColors = _phaseConfigs
                .Where(cfg => cfg.Phase <= phase)
                .SelectMany(cfg => cfg.UnlockedBubbleProjectileColors)
                .Distinct()
                .ToList();

            return unlockedColors;
        }

        [Serializable]
        private class PhaseConfig
        {
            [SerializeField] private int _phase;
            [SerializeField] List<GameObject> _unlockedPresets;
            [SerializeField] private List<BubbleColorType> _unlockedBubbleProjectileColors;

            public int Phase => _phase;
            public List<GameObject> UnlockedPresets => _unlockedPresets;
            public List<BubbleColorType> UnlockedBubbleProjectileColors => _unlockedBubbleProjectileColors;
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