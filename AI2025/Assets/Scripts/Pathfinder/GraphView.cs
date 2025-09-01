using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public class GraphView : MonoBehaviour
    {
        [Header("Debug")] [SerializeField]
        private Color startColour = Color.blue;
        [SerializeField] private Color targetColour = Color.yellow;
        [SerializeField] private Color blockedColour = Color.red;
        [SerializeField] private Color defaultColour = Color.green;
        [SerializeField] private Vector3 cellDrawSize = new(.9f, .9f, 0);
        
        [Header("Settings")] 
        [SerializeField] private List<Vector2Int> blockedNodeCoordinates = new();
        [SerializeField] private Vector2Int size = new(10, 10);
        
        public Graph<Node<Coordinate>, Coordinate> Graph { get; private set; }

        private Node<Coordinate> startNode;
        private Node<Coordinate> targetNode;
        
        private void Start()
        {
            Graph = new Graph<Node<Coordinate>, Coordinate>(size.x, size.y);
            
            List<Coordinate> blockedCoordinates = new();
            
            foreach (Vector2Int blockedNodeCoordinate in blockedNodeCoordinates)
                blockedCoordinates.Add(new Coordinate(blockedNodeCoordinate.x, blockedNodeCoordinate.y));
            
            Graph.BlockNodes(blockedCoordinates);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            
            foreach (Node<Coordinate> node in Graph.Nodes.Values)
            {
                Gizmos.color = node.IsBlocked() ? blockedColour : defaultColour;

                if (node.Equals(startNode))
                    Gizmos.color = startColour;
                
                if (node.Equals(targetNode))
                    Gizmos.color = targetColour;
                
                Gizmos.DrawWireCube(new Vector3(node.GetCoordinate().X, node.GetCoordinate().Y), cellDrawSize);
            }
        }

        public void SetStartNode(Node<Coordinate> node)
        {
            startNode = node;
        }

        public void SetTargetNode(Node<Coordinate> node)
        {
            targetNode = node;
        }
    }
}
