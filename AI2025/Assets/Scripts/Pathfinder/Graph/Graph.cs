using System.Collections.Generic;
using Pathfinder.Coordinate;
using Pathfinder.Node;

namespace Pathfinder.Graph
{
    public class Graph<TNode, TCoordinate> : IGraph<TNode, TCoordinate> 
        where TNode : INode<TCoordinate>, INode, new() 
        where TCoordinate : Coordinate.Coordinate, new()
    {
        public Dictionary<TCoordinate, TNode> Nodes { get; } = new();

        public Graph(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    TCoordinate coordinate = new();
                    TNode node = new();

                    coordinate.Set(i, j);
                    node.SetCoordinate(coordinate);
                    Nodes.Add(coordinate, node);
                }
            }
        }

        public ICollection<TNode> GetAdjacents(TNode node)
        {
            List<TNode> adjacents = new();

            foreach (ICoordinate adjacentCoordinate in node.GetCoordinate().GetAdjacents())
            {
                if (adjacentCoordinate is not TCoordinate coordinate) continue;

                if (Nodes.TryGetValue(coordinate, out TNode adjacentNode))
                    adjacents.Add(adjacentNode);
            }

            return adjacents;
        }

        public ICollection<TNode> GetNodes()
        {
            return Nodes.Values;
        }

        public void BlockNodes(List<TCoordinate> coordinates)
        {
            foreach (TCoordinate coordinate in coordinates)
                Nodes[coordinate].SetBlocked(true);
        }
    }
}