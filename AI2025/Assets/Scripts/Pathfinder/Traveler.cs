using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public class Traveler : MonoBehaviour
    {
        [SerializeField] private GraphView graphView;

        private DepthFirstPathfinder<Node<Coordinate>, Coordinate> pathfinder;
        //private BreadthFirstPathfinder<Node<Vector2Int>> Pathfinder;
        //private DijstraPathfinder<Node<Vector2Int>> Pathfinder;
        //private AStarPathfinder<Node<Vector2Int>> Pathfinder;

        private Node<Coordinate> startNode;
        private Node<Coordinate> destinationNode;

        private void Start()
        {
            pathfinder = new DepthFirstPathfinder<Node<Coordinate>, Coordinate>();

            startNode = new Node<Coordinate>();
            startNode.SetCoordinate(new Coordinate(Random.Range(0, 10), Random.Range(0, 10)));

            destinationNode = new Node<Coordinate>();
            
            int counter = 0;
            
            do
            {
                destinationNode.SetCoordinate(new Coordinate(Random.Range(0, 10), Random.Range(0, 10)));
            } while (graphView.Graph.Nodes[destinationNode.GetCoordinate()].IsBlocked() && counter++ < 100);

            graphView.SetStartNode(startNode);
            graphView.SetTargetNode(destinationNode);
            
            List<Node<Coordinate>> path = pathfinder.FindPath(startNode, destinationNode, graphView.Graph);
            StartCoroutine(Move(path));
        }

        public IEnumerator Move(List<Node<Coordinate>> path)
        {
            foreach (Node<Coordinate> node in path)
            {
                transform.position = new Vector3(node.GetCoordinate().X, node.GetCoordinate().Y);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}