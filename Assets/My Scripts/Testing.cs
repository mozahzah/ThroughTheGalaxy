using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Testing : MonoBehaviour
{
    PathFinding pathFinding;
    //Vector3 endLocation;
    private void Start() 
    {
        pathFinding = new PathFinding(10,10,10);
        
    }

    private void Update() 
    {
        PathNode pathNode = pathFinding.GetGrid().GetValue(new Vector3(20,20,20));
        Debug.Log(pathNode.x + ", " + pathNode.y +","+ pathNode.z);
        List<PathNode> path = pathFinding.FindPath(new Vector3(0,0,0), new Vector3(20,20,20));
        
        if (path != null)
        {
            for (int i = 0; i < path.Count -1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x,path[i].y,path[i].z)* 10f + Vector3.one * 5f, 
                new Vector3(path[i+1].x,path[i+1].y,path[i+1].z)* 10f + Vector3.one * 5f, Color.red, 100f);
            }
        }

        Debug.DrawLine(new Vector3(0,0,0), new Vector3(20,20,20), Color.blue, 100f);
    }
}
