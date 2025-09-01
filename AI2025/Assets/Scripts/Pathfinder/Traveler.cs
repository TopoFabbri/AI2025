using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinder
{
    public class Traveler : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GraphView graphView;

        [Header("Settings")] [SerializeField] private float speed = 1;

        private DepthFirstPathfinder<Node<Coordinate>, Coordinate> pathfinder;
        //private BreadthFirstPathfinder<Node<Vector2Int>> Pathfinder;
        //private DijstraPathfinder<Node<Vector2Int>> Pathfinder;
        //private AStarPathfinder<Node<Vector2Int>> Pathfinder;

        private Node<Coordinate> startNode;
        private Node<Coordinate> destinationNode;
        private Node<Coordinate> currentNode;

        private void OnEnable()
        {
            InputListener.ResetPath += ResetPath;
        }

        private void OnDisable()
        {
            InputListener.ResetPath -= ResetPath;
        }

        private void Start()
        {
            pathfinder = new DepthFirstPathfinder<Node<Coordinate>, Coordinate>();

            startNode = new Node<Coordinate>();

            int counter = 0;

            do
            {
                startNode.SetCoordinate(new Coordinate(Random.Range(0, 10), Random.Range(0, 10)));
            } while (graphView.Graph.Nodes[startNode.GetCoordinate()].IsBlocked() && counter++ < 100);

            FindDestination();
            
            graphView.SetStartNode(startNode);
            graphView.SetTargetNode(destinationNode);

            List<Node<Coordinate>> path = pathfinder.FindPath(startNode, destinationNode, graphView.Graph);
            StartCoroutine(Move(path));
        }

        public IEnumerator Move(List<Node<Coordinate>> path)
        {
            transform.position = new Vector3(startNode.GetCoordinate().X, startNode.GetCoordinate().Y);
            
            foreach (Node<Coordinate> node in path)
            {
                yield return new WaitForSeconds(1.0f / speed);
                currentNode = node;
                transform.position = new Vector3(node.GetCoordinate().X, node.GetCoordinate().Y);
            }
        }

        private void ResetPath()
        {
            startNode = currentNode;
            FindDestination();
                        
            graphView.SetStartNode(startNode);
            graphView.SetTargetNode(destinationNode);
            
            StopAllCoroutines();
            StartCoroutine(Move(pathfinder.FindPath(startNode, destinationNode, graphView.Graph)));
        }

        private void FindDestination()
        {
            destinationNode = new Node<Coordinate>();

            int counter = 0;

            do
            {
                destinationNode.SetCoordinate(new Coordinate(Random.Range(0, 10), Random.Range(0, 10)));
            } while (graphView.Graph.Nodes[destinationNode.GetCoordinate()].IsBlocked() && counter++ < 100);
        }
    }
}