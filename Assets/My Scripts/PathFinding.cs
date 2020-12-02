using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;


    public AStarGrid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;


    public PathFinding(int width, int height, int depth)
    {
       grid = new AStarGrid<PathNode>(width, height, depth, 10f, (AStarGrid<PathNode> g, int x, int y, int z) => new PathNode(g,x,y,z));
    }

    public AStarGrid<PathNode> GetGrid()
    {
        return grid;
    }
    public List<PathNode> FindPath(Vector3 start, Vector3 end)
    {
        PathNode startNode = grid.GetValue(start);
        PathNode endNode = grid.GetValue(end);
        openList = new List<PathNode>{startNode};
        closedList = new List<PathNode>();

        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                for (int w = 0; w < grid.depth; w++)
                {
                    PathNode pathNode = grid.GetValue(i,j,w);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.previousNode = null;
                }
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode,endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);


            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }   
        }

        // out of open list
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

            // Back Plane
            if (currentNode.z - 1 >= 0)
            {
                GetNeighbourPlane(currentNode, neighbourList, currentNode.z -1);
            }
            // Front Plane
            if (currentNode.z + 1 < grid.depth)
            {
                GetNeighbourPlane(currentNode, neighbourList, currentNode.z +1);
            }
            // Current Plane
            GetNeighbourPlane(currentNode, neighbourList, currentNode.z);
            
            return neighbourList;
    }

    private void GetNeighbourPlane(PathNode currentNode, List<PathNode> neighbourList, int depthIndex)
    {
        if (currentNode.x - 1 >= 0)
        {
            //left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y, depthIndex));
            //left down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1, depthIndex));
            //left up
            if (currentNode.y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1, depthIndex));
        }
        if (currentNode.x + 1 < grid.width)
        {
            //right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y, depthIndex));
            //Rigth Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1, depthIndex));
            //Right Up
            if (currentNode.y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1, depthIndex));
        }
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1, depthIndex));
        if (currentNode.y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1, depthIndex));
    }

    private PathNode GetNode(int x, int y, int z)
    {
        return grid.GetValue(x,y,z);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }


    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int zDistance = Mathf.Abs(a.z - b.z);
        //int remaining = Mathf.Abs(xDistance - yDistance - zDistance);
        int remaining = Mathf.FloorToInt(Vector3.Distance(new Vector3(a.x,a.y,a.z), new Vector3(b.x,b.y,b.z)));
        return DIAGONAL_COST * Mathf.Min(xDistance,yDistance,zDistance) + STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for (int i = 1; i < pathNodes.Count; i++){
            if (pathNodes[i].fCost < lowestFCostNode.fCost){
                lowestFCostNode = pathNodes[i];
            }
        }
        return lowestFCostNode;
    }   


}
