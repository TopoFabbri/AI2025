using System;

namespace Pathfinder
{
    public interface ICoordinate
    {
        int GetDistanceTo(ICoordinate other);
    }
}