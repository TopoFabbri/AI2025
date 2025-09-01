using System;
using System.Collections.Generic;

namespace Pathfinder.Coordinate
{
    public interface ICoordinate : IEquatable<ICoordinate>
    {
        int GetDistanceTo(ICoordinate other);
        List<ICoordinate> GetAdjacents();
    }
}