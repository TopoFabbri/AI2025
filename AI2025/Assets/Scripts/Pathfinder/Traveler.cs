using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public class Traveler : MonoBehaviour
    {
        public GraphView graphView;
    
        private DepthFirstPathfinder<Node<Vector2Int>> pathfinder;
        //private BreadthFirstPathfinder<Node<Vector2Int>> Pathfinder;
        //private DijstraPathfinder<Node<Vector2Int>> Pathfinder;
        //private AStarPathfinder<Node<Vector2Int>> Pathfinder;

        private Node<Vector2Int> startNode; 
        private Node<Vector2Int> destinationNode;

        private void Start()
        {
            pathfinder = new DepthFirstPathfinder<Node<Vector2Int>>();
        
            startNode = new Node<Vector2Int>();
            startNode.SetCoordinate(new Vector2Int(Random.Range(0, 10), Random.Range(0, 10)));

            destinationNode = new Node<Vector2Int>();
            destinationNode.SetCoordinate(new Vector2Int(Random.Range(0, 10), Random.Range(0, 10)));

            List<Node<Vector2Int>> path = pathfinder.FindPath(startNode, destinationNode, graphView.graph.nodes);
            StartCoroutine(Move(path));
        }

        public IEnumerator Move(List<Node<Vector2Int>> path) 
        {
            foreach (Node<Vector2Int> node in path)
            {
                transform.position = new Vector3(node.GetCoordinate().x, node.GetCoordinate().y);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
