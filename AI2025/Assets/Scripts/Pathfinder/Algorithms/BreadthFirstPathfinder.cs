using System.Collections.Generic;
using Pathfinder.Coordinate;
using Pathfinder.Graph;
using Pathfinder.Node;

namespace Pathfinder.Algorithms
{
    public class BreadthFirstPathfinder<TNodeType, TCoordinate> : Pathfinder<TNodeType, TCoordinate> 
        where TCoordinate : ICoordinate 
        where TNodeType : INode<TCoordinate>, INode
    {

        protected override bool IsBlocked(TNodeType node)
        {
            return node.IsBlocked();
        }

        protected override int MoveToNeighborCost(TNodeType a, TNodeType b)
        {
            return 0;
        }

        protected override ICollection<TNodeType> GetAdjacents(TNodeType node, IGraph<TNodeType, TCoordinate> graph)
        {
            return graph.GetAdjacents(node);
        }
    }
}
