using TMPro;
using UnityEngine;

namespace Pathfinder.Graph
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private TextMeshProUGUI costLabel;
        
        public void SetValues(Material mat, Vector3 drawSize, int cost)
        {
            SetValues(mat, drawSize);

            costLabel.enabled = true;
            costLabel.text = cost.ToString();
            
            Color color = mat.color;
            color.r = cost / 100f * mat.color.r;
            color.g = cost / 100f * mat.color.g;
            color.b = cost / 100f * mat.color.b;
            
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