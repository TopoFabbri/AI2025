using System.Collections;
using System.Collections.Generic;
using Input;
using Pathfinder.Algorithms;
using Pathfinder.Graph;
using Pathfinder.Node;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinder.Pawn
{
    public class Traveler : MonoBehaviour
    {
        private static int travelerCount = 0;
        
        [Header("References")] [SerializeField]
        private GraphView graphView;

        [Header("Settings")] [SerializeField] private float speed = 1;

        // private DepthFirstPathfinder<Node<Coordinate.Coordinate>, Coordinate.Coordinate> pathfinder;
        // private BreadthFirstPathfinder<Node<Coordinate.Coordinate>, Coordinate.Coordinate> pathfinder;
        // private DijkstraPathfinder<Node<Coordinate.Coordinate>, Coordinate.Coordinate> pathfinder;
        private AStarPathfinder<Node<Coordinate.Coordinate>, Coordinate.Coordinate> pathfinder;

        private Node<Coordinate.Coordinate> startNode;
        private Node<Coordinate.Coordinate> destinationNode;
        private Node<Coordinate.Coordinate> currentNode;

        private int travelerId;
        private bool moveEnded;

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
            travelerId = travelerCount;
            travelerCount++;
            
            pathfinder = new AStarPathfinder<Node<Coordinate.Coordinate>, Coordinate.Coordinate>();

            startNode = new Node<Coordinate.Coordinate>();
            currentNode = startNode;

            int counter = 0;

            do
            {
                startNode.SetCoordinate(new Coordinate.Coordinate(Random.Range(0, 50), Random.Range(0, 50)));
            } while (graphView.Graph.Nodes[startNode.GetCoordinate()].IsBlocked() && counter++ < 100);

            FindDestination();

            graphView.SetStartNode(travelerId, startNode);
            graphView.SetTargetNode(travelerId, destinationNode);

            List<Node<Coordinate.Coordinate>> path = pathfinder.FindPath(startNode, destinationNode, graphView.Graph);
            StartCoroutine(Move(path));
        }

        private void Update()
        {
            if (moveEnded)
                ResetPath();
        }

        public IEnumerator Move(List<Node<Coordinate.Coordinate>> path)
        {
            transform.position = new Vector3(startNode.GetCoordinate().X, startNode.GetCoordinate().Y);

            foreach (Node<Coordinate.Coordinate> node in path)
            {
                float time = 0f;
                Vector3 lastPos = new(currentNode.GetCoordinate().X, currentNode.GetCoordinate().Y);
                Vector3 nextPos = new(node.GetCoordinate().X, node.GetCoordinate().Y);

                while (time < 1f)
                {
                    time += Time.deltaTime * speed;
                    transform.position = Vector3.Lerp(lastPos, nextPos, time);

                    yield return null;
                }

                currentNode = node;
                
                if (currentNode.GetCost() + 2  > 99)
                    currentNode.SetBlocked(true);
                else
                    currentNode.SetCost(currentNode.GetCost() + 2);
            }

            moveEnded = true;
        }

        private void ResetPath()
        {
            startNode = currentNode;
            FindDestination();

            graphView.SetStartNode(travelerId, startNode);
            graphView.SetTargetNode(travelerId, destinationNode);

            moveEnded = false;

            StopAllCoroutines();
            StartCoroutine(Move(pathfinder.FindPath(startNode, destinationNode, graphView.Graph)));
        }

        private void FindDestination()
        {
            destinationNode = new Node<Coordinate.Coordinate>();

            int counter = 0;

            do
            {
                destinationNode.SetCoordinate(new Coordinate.Coordinate(Random.Range(0, 50), Random.Range(0, 50)));
            } while (graphView.Graph.Nodes[destinationNode.GetCoordinate()].IsBlocked() && counter++ < 100);
        }
    }
}