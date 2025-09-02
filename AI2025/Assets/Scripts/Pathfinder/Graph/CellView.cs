using TMPro;
using UnityEngine;

namespace Pathfinder.Graph
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshProUGUI costLabel;
        [SerializeField] private bool drawCosts;
        [SerializeField] private bool drawNumbers;

        private Color colour = Color.white;
        private Vector3 scale = Vector3.one;
        private string cellText = "";
        
        public void SetValues(Color colour, Vector3 drawSize, int cost)
        {
            SetValues(colour, drawSize);

            if (drawNumbers)
                cellText = cost.ToString();

            if (!drawCosts) return;

            colour.r = cost / 100f;
            colour.g = (100 - cost) / 100f;
            colour.b = 0;

            this.colour = colour;
        }

        public void SetValues(Color colour, Vector3 drawSize)
        {
            this.colour = colour;

            scale = drawSize;
        }

        private void LateUpdate()
        {
            spriteRenderer.color = colour;
            transform.localScale = scale;
            costLabel.enabled = drawNumbers;
            costLabel.text = cellText;
        }
    }
}