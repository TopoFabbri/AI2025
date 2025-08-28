using System.Collections.Generic;

namespace Pathfinder
{
    public class DijkstraPathfinder<TNodeType> : Pathfinder<TNodeType> where TNodeType : INode
    {
        protected override int Distance(TNodeType a, TNodeType b)
        {
            throw new System.NotImplementedException();
        }

        protected override ICollection<TNodeType> GetNeighbors(TNodeType node)
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsBloqued(TNodeType node)
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
