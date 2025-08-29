using System.Collections.Generic;

namespace Pathfinder
{
    public class Vector2IntGraph<TNodeType> 
        where TNodeType : INode<Coordinate>, INode, new()
    { 
        public readonly List<TNodeType> nodes = new();

        public Vector2IntGraph(int x, int y) 
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    TNodeType node = new();
                    node.SetCoordinate(new Coordinate(i, j));
                    nodes.Add(node);
                }
            }
        }

    }
}