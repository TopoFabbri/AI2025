using System;

namespace Pathfinder
{
    public class Node<TCoordinate> : INode, INode<TCoordinate> where TCoordinate : ICoordinate
    {
        private TCoordinate coordinate;
        private bool blocked;

        public void SetCoordinate(TCoordinate coordinate)
        {
            this.coordinate = coordinate;
        }

        public TCoordinate GetCoordinate()
        {
            return coordinate;
        }

        public bool IsBlocked()
        {
            return blocked;
        }

        public void SetBlocked(bool blocked)
        {
            this.blocked = blocked;
        }

        public bool Equals(INode<TCoordinate> other)
        {
            return other != null && coordinate.Equals(other.GetCoordinate());
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Node<TCoordinate>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(coordinate.GetHashCode(), blocked);
        }
    }
}