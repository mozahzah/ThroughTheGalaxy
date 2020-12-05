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

    public bool isValid;
    public PathNode previousNode;
    [SerializeField] public GameObject gameObjectNode;

    public PathNode(AStarGrid<PathNode> grid, int x, int y, int z)
    {
        UnityEngine.Object pPrefab = Resources.Load("GameObjectNode");
        gameObjectNode = (GameObject)GameObject.Instantiate(pPrefab);
        gameObjectNode.transform.localScale = new Vector3(10,10,10);
        gameObjectNode.transform.position = new Vector3(x,y,z)*10 + Vector3.one * 5f;
        var collider = gameObjectNode.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
        isValid = gameObjectNode.GetComponent<GameObjectNode>().isValid;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(this.x,this.y,this.z) * 10f + Vector3.one * 5f;
    }
}
