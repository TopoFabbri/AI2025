using System.Collections.Generic;
using Pathfinder.Coordinate;
using Pathfinder.Node;

namespace Pathfinder.Graph
{
    public interface IGraph<TNode, TCoordinate> where TNode : INode<TCoordinate>, 
        INode where TCoordinate : ICoordinate
    {
        public ICollection<TNode> GetNodes();
        public TCoordinate GetSize();
        public ICollection<TNode> GetAdjacents(TNode node);
        public void BlockNodes(ICollection<TCoordinate> nodes);
        public void BlockNodes(ICollection<TNode> nodes);
        public ICollection<TNode> GetBresenhamNodes(TCoordinate start, TCoordinate end);
    }
}