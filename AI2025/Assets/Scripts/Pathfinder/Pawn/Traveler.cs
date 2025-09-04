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
        private static int _travelerCount;
        
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
            travelerId = _travelerCount;
            _travelerCount++;
            
            pathfinder = new AStarPathfinder<Node<Coordinate.Coordinate>, Coordinate.Coordinate>();

            startNode = new Node<Coordinate.Coordinate>();
            currentNode = startNode;

            int counter = 0;

            do
            {
                startNode.SetCoordinate(new Coordinate.Coordinate(Random.Range(0, graphView.Graph.GetSize().X), Random.Range(0, graphView.Graph.GetSize().Y)));
            } while (graphView.Graph.Nodes[startNode.GetCoordinate()].IsBlocked() && counter++ < 100);

            ResetPath();
        }

        private void Update()
        {
            if (moveEnded)
                ResetPath();
        }

        private IEnumerator Move(List<Node<Coordinate.Coordinate>> path)
        {
            transform.position = new Vector3(startNode.GetCoordinate().X, startNode.GetCoordinate().Y);
            
            foreach (Node<Coordinate.Coordinate> node in path)
            {
                Vector3 nextPos = new(node.GetCoordinate().X, node.GetCoordinate().Y);

                while (transform.position != nextPos)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

                    yield return null;
                }

                currentNode = node;
            }

            moveEnded = true;
        }

        private void ResetPath()
        {
            startNode = currentNode;
            FindDestination();

            graphView.SetTargetNode(travelerId, destinationNode);

            moveEnded = false;

            List<Node<Coordinate.Coordinate>> path = pathfinder.FindPath(startNode, destinationNode, graphView.Graph);
            List<Node<Coordinate.Coordinate>> fullPath = GetFullPath(path);
            
            if (fullPath != null && fullPath.Count > 0)
            {
                graphView.ClearPathNodes(travelerId);
                graphView.AddPathNodes(travelerId, fullPath);

                foreach (Node<Coordinate.Coordinate> node in fullPath)
                {
                    if (node.GetCost() + 2  > 99)
                        node.SetCost(0);
                    else
                        node.SetCost(node.GetCost() + 2);
                }
            }

            StopAllCoroutines();
            StartCoroutine(Move(path));
        }

        private void FindDestination()
        {
            destinationNode = new Node<Coordinate.Coordinate>();

            int counter = 0;

            do
            {
                destinationNode.SetCoordinate(new Coordinate.Coordinate(Random.Range(0, graphView.Graph.GetSize().X), Random.Range(0, graphView.Graph.GetSize().Y)));
            } while (graphView.Graph.Nodes[destinationNode.GetCoordinate()].IsBlocked() && counter++ < 100);
        }

        private List<Node<Coordinate.Coordinate>> GetFullPath(List<Node<Coordinate.Coordinate>> path)
        {
            if (path == null) return null;
            if (path.Count == 0) return path;
            
            List<Node<Coordinate.Coordinate>> fullPath = new();

            fullPath.AddRange(graphView.Graph.GetBresenhamNodes(startNode.GetCoordinate(), path[0].GetCoordinate()));
            
            for (int i = 0; i < path.Count - 1; i++)
            {
                fullPath.AddRange(graphView.Graph.GetBresenhamNodes(path[i].GetCoordinate(), path[i + 1].GetCoordinate()));
            }
            
            return fullPath;
        }
    }
}