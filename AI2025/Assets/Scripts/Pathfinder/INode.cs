namespace Pathfinder
{
    public interface INode
    {
        public bool IsBlocked();
    }

    public interface INode<TCoordinate> where TCoordinate : ICoordinate 
    {
        public void SetCoordinate(TCoordinate coordinateType);
        public TCoordinate GetCoordinate();
    }
}