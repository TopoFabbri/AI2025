namespace Pathfinder
{
    public class Node<TCoordinate> : INode, INode<TCoordinate> where TCoordinate : ICoordinate
    {
        private TCoordinate coordinate;

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
            return false;
        }
    }
}