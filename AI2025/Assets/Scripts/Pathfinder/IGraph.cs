using System.Collections.Generic;

namespace Pathfinder
{
    public interface IGraph<TNode, TCoordinate> where TNode : INode<TCoordinate>, 
        INode where TCoordinate : ICoordinate
    {
        public ICollection<TNode> GetNodes();
        public ICollection<TNode> GetAdjacents(TNode node);
        public void BlockNodes(List<TCoordinate> nodes);
    }
}