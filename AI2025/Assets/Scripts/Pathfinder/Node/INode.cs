using System;
using Pathfinder.Coordinate;

namespace Pathfinder.Node
{
    public interface INode
    {
        public bool IsBlocked();
        public int GetCost();
        public void SetBlocked(bool blocked);
        public void SetCost(int cost);
    }

    public interface INode<TCoordinate> : IEquatable<INode<TCoordinate>> where TCoordinate : ICoordinate
    {
        public void SetCoordinate(TCoordinate coordinateType);
        public TCoordinate GetCoordinate();
    }
}