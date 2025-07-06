using System.Collections.Generic;
using UnityEngine;

namespace NotBubbleFall.Gameplay
{
    public class BubblePresset : MonoBehaviour
    {
        [SerializeField] private List<BubbleColorType> _disallowedRandomColors;

        private IFieldController _fieldController;

        public void Initialize()
        {
            var allowedColors = _fieldController.GetAllowedColors();
            allowedColors.RemoveAll(color => _disallowedRandomColors.Contains(color));

            foreach (Transform row in transform)
            {
                foreach (Transform bubbleTransform in row)
                {
                    var bubble = bubbleTransform.GetComponent<Bubble>();

                    if (bubble.BubbleColor == BubbleColorType.Default)
                    {
                        bubble.SetColor(GetRandomBubbleColor(allowedColors));
                    }
                }
            }
        }

        private void Awake()
        {
            _fieldController = ServiceLocator.Resolve<IFieldController>();
        }

        private BubbleColorType GetRandomBubbleColor(List<BubbleColorType> from)
        {
            return from[Random.Range(0, from.Count)];
        }
    }
}

