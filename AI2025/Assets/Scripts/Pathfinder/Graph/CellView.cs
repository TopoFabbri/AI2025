using TMPro;
using UnityEngine;

namespace Pathfinder.Graph
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private TextMeshProUGUI costLabel;
        [SerializeField] private bool drawCosts;
        
        public void SetValues(Material mat, Vector3 drawSize, int cost)
        {
            SetValues(mat, drawSize);

            if (!drawCosts) return;
            
            costLabel.enabled = true;
            costLabel.text = cost.ToString();

            Color color = mat.color;
            color.r = cost / 100f;
            color.g = (100 - cost) / 100f;
            color.b = 0;

            meshRenderer.material.color = color;
        }

        public void SetValues(Material mat, Vector3 drawSize)
        {
            meshRenderer.material = mat;
            transform.localScale = drawSize;

            costLabel.enabled = false;
        }
    }
}