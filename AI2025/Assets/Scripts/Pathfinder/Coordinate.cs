using System;

namespace Pathfinder
{
    public struct Coordinate : ICoordinate 
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int GetDistanceTo(ICoordinate coordinate)
        {
            if (!typeof(Coordinate).IsAssignableFrom(coordinate.GetType())) return -1;
            
            return Math.Abs(X - ((Coordinate)coordinate).X)  + Math.Abs(Y - ((Coordinate)coordinate).Y);
        }

        public void SetCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}