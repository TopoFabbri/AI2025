using System.Collections.Generic;

namespace Pathfinder
{
    public class Vector2IntGraph<TNode, TCoordinate> : IGraph<TNode, TCoordinate> 
        where TNode : INode<TCoordinate>, INode, new() 
        where TCoordinate : Coordinate, new()
    {
        public Dictionary<TCoordinate, TNode> Nodes { get; } = new();

        public Vector2IntGraph(int x, int y)
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
    }
}