using System;
using System.Collections.Generic;
using UnityEngine;

namespace NotBubbleFall.Data
{
    public enum SpecialProjectileType
    {
        Bomb,
        Rocket
    }

    [CreateAssetMenu(fileName = "BubbleData", menuName = "Data/ProjectileDB", order = 1)]
    public class ProjectileDB : ScriptableObject, IService
    {
        [SerializeField] private GameObject _bubbleProjectilePrefab;
        [SerializeField] private List<SpecialProjectileData> _specialProjectilesData;

        public GameObject BubbleProjectilePrefab => _bubbleProjectilePrefab;

        public GameObject GetSpecialProjectilePrefab(SpecialProjectileType type)
        {
            return _specialProjectilesData.Find(data => data.Type == type)?.Prefab;
        }

        [Serializable]
        private class SpecialProjectileData
        {
            [SerializeField] private SpecialProjectileType _type;
            [SerializeField] private GameObject _prefab;

            public SpecialProjectileType Type => _type;
            public GameObject Prefab => _prefab;
        }
    }
}