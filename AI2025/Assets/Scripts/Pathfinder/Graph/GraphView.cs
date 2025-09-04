using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinder.Node;
using UnityEngine;

namespace Pathfinder.Graph
{
    public class GraphView : MonoBehaviour
    {
        [Header("Drawing")] [SerializeField] private CellView tile;
        [SerializeField] private Color defaultColour;
        [SerializeField] private Color startColour;
        [SerializeField] private Color destinationColour;
        [SerializeField] private Color blockedColour;
        [SerializeField] private Vector3 cellDrawSize = new(.9f, .9f, 0);

        [Header("Settings")] [SerializeField] private List<Vector2Int> blockedNodeCoordinates = new();
        [SerializeField] private Vector2Int size = new(10, 10);
        
        public Graph<Node<Coordinate.Coordinate>, Coordinate.Coordinate> Graph { get; private set; }

        private readonly Dictionary<int, Node<Coordinate.Coordinate>> startNodes = new();
        private readonly Dictionary<int, Node<Coordinate.Coordinate>> targetNodes = new();
        
        private readonly ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 32 };
        
        private readonly Dictionary<Coordinate.Coordinate, CellView> cellViews = new();

        private void Awake()
        {
            Graph = new Graph<Node<Coordinate.Coordinate>, Coordinate.Coordinate>(size.x, size.y);

            foreach (KeyValuePair<Coordinate.Coordinate, Node<Coordinate.Coordinate>> node in Graph.Nodes)
            {
                int cost = 0;// Random.Range(0, costRange);
                    
                node.Value.SetCost(cost);
                CellView cellInstance = Instantiate(tile, new Vector3(node.Key.X, node.Key.Y, 0), Quaternion.identity);
                cellInstance.transform.SetParent(transform);
                cellInstance.name = "Cell (" + node.Key.X + ", " + node.Key.Y + ")";
                cellViews.Add(node.Key, cellInstance);
            }

            List<Coordinate.Coordinate> blockedCoordinates = new();
            
            foreach (Vector2Int blockedNodeCoordinate in blockedNodeCoordinates)
                blockedCoordinates.Add(new Coordinate.Coordinate(blockedNodeCoordinate.x, blockedNodeCoordinate.y));

            Graph.BlockNodes(Graph.GetBresenhamNodes(blockedCoordinates[0], blockedCoordinates[1]));
        }

        private void LateUpdate()
        {
            Parallel.ForEach(Graph.Nodes, parallelOptions, node =>
            {
                if (startNodes.ContainsValue(node.Value))
                    cellViews[node.Key].SetValues(startColour, cellDrawSize);
                else if (targetNodes.ContainsValue(node.Value))
                    cellViews[node.Key].SetValues(destinationColour, cellDrawSize);
                else if (node.Value.IsBlocked())
                    cellViews[node.Key].SetValues(blockedColour, cellDrawSize);
                else
                    cellViews[node.Key].SetValues(defaultColour, cellDrawSize, node.Value.GetCost());
            });
            
            // foreach (KeyValuePair<Coordinate.Coordinate, Node<Coordinate.Coordinate>> node in Graph.Nodes)
            // {
            //     if (startNodes.ContainsValue(node.Value))
            //         cellViews[node.Key].SetValues(startColour, cellDrawSize);
            //     else if (targetNodes.ContainsValue(node.Value))
            //         cellViews[node.Key].SetValues(destinationColour, cellDrawSize);
            //     else if (node.Value.IsBlocked())
            //         cellViews[node.Key].SetValues(blockedColour, cellDrawSize);
            //     else
            //         cellViews[node.Key].SetValues(defaultColour, cellDrawSize, node.Value.GetCost());
            // }
        }
        
        public void SetStartNode(int traveler, Node<Coordinate.Coordinate> node)
        {
            startNodes[traveler] = node;
        }

        public void SetTargetNode(int traveler, Node<Coordinate.Coordinate> node)
        {
            targetNodes[traveler] = node;
        }
    }
}