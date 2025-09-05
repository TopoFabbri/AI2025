using System.Collections.Generic;
using Pathfinder.Coordinate;
using Pathfinder.Graph;
using Pathfinder.Node;

namespace Pathfinder.Algorithms
{
    public class AStarPathfinder<TNodeType, TCoordinate> : Pathfinder<TNodeType, TCoordinate> 
        where TCoordinate : ICoordinate 
        where TNodeType : INode<TCoordinate>, INode
    {
        public override List<TNodeType> FindPath(TNodeType startNode, TNodeType destinationNode, IGraph<TNodeType, TCoordinate> graph)
        {
            List<TNodeType> astarPath = base.FindPath(startNode, destinationNode, graph);
            
            if (astarPath == null || astarPath.Count == 0) return astarPath;
            List<TNodeType> newPath = new();

            TNodeType currentNode = startNode;
            int counter = 0;
            
            do
            {
                ICollection<TNodeType> bresenhamPath = graph.GetBresenhamNodes(currentNode.GetCoordinate(), astarPath[counter].GetCoordinate());
                
                bool blocked = false;

                foreach (TNodeType node in bresenhamPath)
                {
                    if (!node.IsBlocked()) continue;
                    
                    blocked = true;
                    break;
                }
                
                if (blocked)
                {
                    newPath.Add(astarPath[counter - 1]);
                    currentNode = astarPath[counter - 1];
                }
                else
                {
                    if (counter == astarPath.Count - 1)
                        newPath.Add(astarPath[counter]);
                    
                    counter++;
                }
                
            } while (counter < astarPath.Count);
            
            return newPath;
        }

        protected override int MoveToNeighborCost(TNodeType a, TNodeType b)
        {
            return b.GetCost();
        }

        protected override ICollection<TNodeType> GetAdjacents(TNodeType node, IGraph<TNodeType, TCoordinate> graph)
        {
            return graph.GetAdjacents(node);
        }
    }
}
