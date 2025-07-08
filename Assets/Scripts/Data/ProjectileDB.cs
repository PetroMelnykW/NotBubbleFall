using UnityEngine;

namespace NotBubbleFall.Data
{
    [CreateAssetMenu(fileName = "BubbleData", menuName = "Data/ProjectileDB", order = 1)]
    public class ProjectileDB : ScriptableObject, IService
    {
        [SerializeField] private GameObject _bubbleProjectilePrefab;
        
        public GameObject BubbleProjectilePrefab => _bubbleProjectilePrefab;
    }
}