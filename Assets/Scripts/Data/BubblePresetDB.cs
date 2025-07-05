using System;
using System.Collections.Generic;
using UnityEngine;

namespace NotBubbleFall.Data
{
    [CreateAssetMenu(fileName = "BubblePresetDB", menuName = "Data/BubblePresetDB")]

    public class BubblePresetDB : ScriptableObject, IService
    {
        [SerializeField] private List<PhasePresets> _phasePresets;

        public GameObject GetRandomPresset(int phase)
        {
            var availablePresets = new List<GameObject>();

            for (int i = 0; i < _phasePresets.Count; i++)
            {
                if (_phasePresets[i].Phase <= phase)
                {
                    availablePresets.AddRange(_phasePresets[i].Presets);
                }
            }

            if (availablePresets.Count == 0)
            {
                Debug.LogWarning($"No presets available for phase {phase}");
                return null;
            }

            return availablePresets[UnityEngine.Random.Range(0, availablePresets.Count)];
        }

        [Serializable]
        private class PhasePresets
        {
            [SerializeField] private int _phase;
            [SerializeField] List<GameObject> _presets;

            public int Phase => _phase;
            public List<GameObject> Presets => _presets;
        }
    }
}