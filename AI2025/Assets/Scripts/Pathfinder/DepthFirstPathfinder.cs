using System;

namespace Pathfinder
{
    public class DepthFirstPathfinder<TNodeType, TCoordinate> : Pathfinder<TNodeType, TCoordinate> 
        where TCoordinate : ICoordinate 
        where TNodeType : INode<TCoordinate>, INode
    {
        protected override int Distance(TNodeType a, TNodeType b)
        {
            return a.GetCoordinate().GetDistanceTo(b.GetCoordinate());
        }

        protected override bool IsBlocked(TNodeType node)
        {
            return node.IsBlocked();
        }

        protected override int MoveToNeighborCost(TNodeType a, TNodeType b)
        {
            return 0;
        }

        protected override bool NodesEquals(TNodeType a, TNodeType b)
        {
            return a.Equals(b);
        }
    }
}
