using System.Collections.Generic;
using UnityEngine;

namespace NotBubbleFall
{
    public static class Tools
    {
        public static T PickRandom<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new System.ArgumentNullException(nameof(enumerable), "The enumerable cannot be null.");
            }
            var list = new List<T>(enumerable);
            if (list.Count == 0)
            {
                throw new System.InvalidOperationException("Cannot pick a random element from an empty collection.");
            }
            int randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
    
}
