using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private AStarGrid<PathNode> grid;
    public int x,y,z;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode previousNode;

    public PathNode(AStarGrid<PathNode> grid, int x, int y, int z)
    {
       this.grid = grid;
       this.x = x;
       this.y = y;
       this.z = z;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
