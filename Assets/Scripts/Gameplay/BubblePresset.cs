using System.Collections.Generic;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public class BubblePresset : MonoBehaviour
    {
        [SerializeField] private List<BubbleColorType> _disallowedRandomColors;

        private void Awake()
        {
            var fieldController = ServiceLocator.Resolve<IFieldController>();

            var allowedColors = fieldController.GetAllowedColors();
            allowedColors.RemoveAll(color => _disallowedRandomColors.Contains(color));

            foreach (Transform row in transform)
            {
                foreach (Transform bubbleTransform in row)
                {
                    var bubble = bubbleTransform.GetComponent<Bubble>();
                    bubble.Initialize();

                    if (bubble.BubbleColor == BubbleColorType.Default)
                    {
                        bubble.SetColor(GetRandomBubbleColor(allowedColors));
                    }
                }
            }
        }

        private BubbleColorType GetRandomBubbleColor(List<BubbleColorType> from)
        {
            return from[Random.Range(0, from.Count)];
        }
    }
}

