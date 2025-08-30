using System.Collections.Generic;

namespace Pathfinder
{
    public class AStarPathfinder<TNodeType, TCoordinate> : Pathfinder<TNodeType, TCoordinate> 
        where TCoordinate : ICoordinate 
        where TNodeType : INode<TCoordinate>, INode
    {
        protected override int Distance(TNodeType a, TNodeType b)
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsBlocked(TNodeType node)
        {
            throw new System.NotImplementedException();
        }

        protected override int MoveToNeighborCost(TNodeType a, TNodeType b)
        {
            throw new System.NotImplementedException();
        }

        protected override bool NodesEquals(TNodeType a, TNodeType b)
        {
            throw new System.NotImplementedException();
        }
    }
}
