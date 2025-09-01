using System.Collections.Generic;
using Pathfinder.Node;
using UnityEngine;

namespace Pathfinder.Graph
{
    public class GraphView : MonoBehaviour
    {
        [Header("Drawing")] [SerializeField] private CellView tile;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material startMaterial;
        [SerializeField] private Material destinationMaterial;
        [SerializeField] private Material blockedMaterial;
        [SerializeField] private Vector3 cellDrawSize = new(.9f, .9f, 0);

        [Header("Settings")] [SerializeField] private List<Vector2Int> blockedNodeCoordinates = new();
        [SerializeField] private Vector2Int size = new(10, 10);
        [SerializeField] private int costRange = 100;
        
        public Graph<Node<Coordinate.Coordinate>, Coordinate.Coordinate> Graph { get; private set; }

        private Node<Coordinate.Coordinate> startNode;
        private Node<Coordinate.Coordinate> targetNode;
        
        private readonly Dictionary<Coordinate.Coordinate, CellView> cellViews = new();

        private void Start()
        {
            Graph = new Graph<Node<Coordinate.Coordinate>, Coordinate.Coordinate>(size.x, size.y);

            foreach (KeyValuePair<Coordinate.Coordinate, Node<Coordinate.Coordinate>> node in Graph.Nodes)
            {
                int cost = Random.Range(0, costRange);
                    
                node.Value.SetCost(cost);
                CellView cellInstance = Instantiate(tile, new Vector3(node.Key.X, node.Key.Y, 0), Quaternion.identity);
                cellInstance.transform.SetParent(transform);
                cellViews.Add(node.Key, cellInstance);
            }

            List<Coordinate.Coordinate> blockedCoordinates = new();

            foreach (Vector2Int blockedNodeCoordinate in blockedNodeCoordinates)
                blockedCoordinates.Add(new Coordinate.Coordinate(blockedNodeCoordinate.x, blockedNodeCoordinate.y));

            Graph.BlockNodes(blockedCoordinates);
        }

        private void LateUpdate()
        {
            foreach (KeyValuePair<Coordinate.Coordinate, Node<Coordinate.Coordinate>> node in Graph.Nodes)
            {
                if (node.Value.Equals(startNode))
                    cellViews[node.Key].SetValues(startMaterial, cellDrawSize);
                else if (node.Value.Equals(targetNode))
                    cellViews[node.Key].SetValues(destinationMaterial, cellDrawSize);
                else if (node.Value.IsBlocked())
                    cellViews[node.Key].SetValues(blockedMaterial, cellDrawSize);
                else
                    cellViews[node.Key].SetValues(defaultMaterial, cellDrawSize, node.Value.GetCost());
            }
        }

        public void SetStartNode(Node<Coordinate.Coordinate> node)
        {
            startNode = node;
        }

        public void SetTargetNode(Node<Coordinate.Coordinate> node)
        {
            targetNode = node;
        }
    }
}