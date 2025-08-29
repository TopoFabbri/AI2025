using UnityEngine;

namespace Pathfinder
{
    public class GraphView : MonoBehaviour
    {
        public Vector2IntGraph<Node<Coordinate>> graph;

        private void Start()
        {
            graph = new Vector2IntGraph<Node<Coordinate>>(10, 10);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            foreach (Node<Coordinate> node in graph.nodes)
            {
                Gizmos.color = node.IsBlocked() ? Color.red : Color.green;

                Gizmos.DrawWireCube(new Vector3(node.GetCoordinate().X, node.GetCoordinate().Y), new Vector3(1, 1, 0));
            }
        }
    }
}
