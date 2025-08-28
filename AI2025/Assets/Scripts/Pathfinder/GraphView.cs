using UnityEngine;

namespace Pathfinder
{
    public class GraphView : MonoBehaviour
    {
        public Vector2IntGraph<Node<Vector2Int>> graph;

        private void Start()
        {
            graph = new Vector2IntGraph<Node<Vector2Int>>(10, 10);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            foreach (Node<Vector2Int> node in graph.nodes)
            {
                Gizmos.color = node.IsBlocked() ? Color.red : Color.green;

                Gizmos.DrawWireCube(new Vector3(node.GetCoordinate().x, node.GetCoordinate().y), new Vector3(1, 1, 0));
            }
        }
    }
}
