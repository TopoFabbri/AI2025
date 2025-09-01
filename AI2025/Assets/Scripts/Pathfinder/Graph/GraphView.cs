using System.Collections.Generic;
using Pathfinder.Node;
using UnityEngine;

namespace Pathfinder.Graph
{
    public class GraphView : MonoBehaviour
    {
        [Header("Debug")] [SerializeField] private Mesh tileMesh;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material startMaterial;
        [SerializeField] private Material destinationMaterial;
        [SerializeField] private Material blockedMaterial;
        [SerializeField] private Vector3 cellDrawSize = new(.9f, .9f, 0);

        [Header("Settings")] [SerializeField] private List<Vector2Int> blockedNodeCoordinates = new();
        [SerializeField] private Vector2Int size = new(10, 10);

        public Graph<Node<Coordinate.Coordinate>, Coordinate.Coordinate> Graph { get; private set; }

        private Node<Coordinate.Coordinate> startNode;
        private Node<Coordinate.Coordinate> targetNode;

        private readonly List<Matrix4x4> defaultMatrices = new();
        private readonly List<Matrix4x4> startMatrices = new();
        private readonly List<Matrix4x4> destinationMatrices = new();
        private readonly List<Matrix4x4> blockedMatrices = new();

        private void Start()
        {
            Graph = new Graph<Node<Coordinate.Coordinate>, Coordinate.Coordinate>(size.x, size.y);

            foreach (KeyValuePair<Coordinate.Coordinate, Node<Coordinate.Coordinate>> node in Graph.Nodes)
            {
                int cost = Random.Range(0, 100);
                    
                node.Value.SetCost(cost);
            }

            List<Coordinate.Coordinate> blockedCoordinates = new();

            foreach (Vector2Int blockedNodeCoordinate in blockedNodeCoordinates)
                blockedCoordinates.Add(new Coordinate.Coordinate(blockedNodeCoordinate.x, blockedNodeCoordinate.y));

            Graph.BlockNodes(blockedCoordinates);
        }

        private void LateUpdate()
        {
            defaultMatrices.Clear();
            startMatrices.Clear();
            destinationMatrices.Clear();
            blockedMatrices.Clear();

            foreach (Node<Coordinate.Coordinate> node in Graph.Nodes.Values)
            {
                Vector3 pos = new(node.GetCoordinate().X, node.GetCoordinate().Y, 0);
                Matrix4x4 mat4 = new();
                mat4.SetTRS(pos, Quaternion.identity, cellDrawSize);

                if (node.Equals(startNode))
                    startMatrices.Add(mat4);
                else if (node.Equals(targetNode))
                    destinationMatrices.Add(mat4);
                else if (node.IsBlocked())
                    blockedMatrices.Add(mat4);
                else
                    defaultMatrices.Add(mat4);
            }

            Graphics.DrawMeshInstanced(tileMesh, 0, defaultMaterial, defaultMatrices);
            Graphics.DrawMeshInstanced(tileMesh, 0, startMaterial, startMatrices);
            Graphics.DrawMeshInstanced(tileMesh, 0, destinationMaterial, destinationMatrices);
            Graphics.DrawMeshInstanced(tileMesh, 0, blockedMaterial, blockedMatrices);
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