using System;

namespace Pathfinder
{
    public interface INode
    {
        public bool IsBlocked();
    }

    public interface INode<TCoordinate> : IEquatable<INode<TCoordinate>> where TCoordinate : ICoordinate
    {
        public void SetCoordinate(TCoordinate coordinateType);
        public TCoordinate GetCoordinate();
    }
}