﻿
using UnityEngine;
using System;

public class AStarGrid<TGridObjects>
{
    // Grid Size Settings
    public int width   {get; set;}
    public int height  {get; set;}
    public int depth   {get; set;}
    private float cellSize;

    // Grid Array Variable
    private TGridObjects[,,] gridArray;
    private TextMesh[,,] debugGridArray;

    // Grid CTOR
    public AStarGrid(int x, int y, int z, float cellSize, Func<AStarGrid<TGridObjects>,int,int,int,TGridObjects> createGridObjects)
    {
        width = x;
        height = y;
        depth = z;
        this.cellSize = cellSize;

        gridArray = new TGridObjects[x, y, z];
        debugGridArray = new TextMesh[x, y, z];

        /*  
            Nested for loop going through each cell following the arguments given when we call the constructor.
            In this constructor, we also construct the GridObject
        */
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                for (int w = 0; w < gridArray.GetLength(2); w++)
                {
                    gridArray[i, j, w] = createGridObjects(this, i, j, w);
                }
            }
        }
       // DebugGrid();
    }

    // Getter
    public TGridObjects GetValue(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >=0 && x < width && y < height && z < depth)
        {
            return gridArray[x,y,z]; 
        }
        else {
            return default(TGridObjects);
        }
    }
    public TGridObjects GetValue(Vector3 position)
    {
        return gridArray[Mathf.FloorToInt(position.x/cellSize),Mathf.FloorToInt(position.y/cellSize),Mathf.FloorToInt(position.z/cellSize)];
    }

    // Gizmos Debug
    private void DebugGrid()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                for (int w = 0; w < gridArray.GetLength(2); w++)
                {
                    DrawWorldCube(i, j, w, cellSize);
                    Debug.DrawLine(new Vector3(0, height, w) * cellSize, new Vector3(width, height, w) * cellSize, Color.white, 100f);
                    Debug.DrawLine(new Vector3(width, 0, w) * cellSize, new Vector3(width, height, w) * cellSize, Color.white, 100f);
                }
                Debug.DrawLine(new Vector3(width, j, 0) * cellSize, new Vector3(width, j, depth) * cellSize, Color.white, 100f);
                Debug.DrawLine(new Vector3(0, j, depth) * cellSize, new Vector3(width, j, depth) * cellSize, Color.white, 100f);
            }
            Debug.DrawLine(new Vector3(i, height, 0) * cellSize, new Vector3(i, height, depth) * cellSize, Color.white, 100f);
            Debug.DrawLine(new Vector3(i, height, depth) * cellSize, new Vector3(i, 0, depth) * cellSize, Color.white, 100f);
        }
        Debug.DrawLine(new Vector3(width, 0, depth) * cellSize, new Vector3(width, height, depth) * cellSize, Color.white, 100f);
        Debug.DrawLine(new Vector3(width, gridArray.GetLength(1), 0) * cellSize, new Vector3(width, gridArray.GetLength(1), depth) * cellSize, Color.white, 100f);
        Debug.DrawLine(new Vector3(0, gridArray.GetLength(1), depth) * cellSize, new Vector3(width, gridArray.GetLength(1), depth) * cellSize, Color.white, 100f);
    }
    private void DrawWorldCube(int x, int y, int z, float cellSize)
    {
        Debug.DrawLine(new Vector3(x,y,z)*cellSize, new Vector3(x+1,y,z)*cellSize, Color.white,100f);
        Debug.DrawLine(new Vector3(x,y,z)*cellSize, new Vector3(x,y+1,z)*cellSize, Color.white,100f);
        Debug.DrawLine(new Vector3(x,y,z)*cellSize, new Vector3(x,y,z+1)*cellSize, Color.white,100f);
    }
    private TextMesh DrawWorldText(int x, int y, int z, float cellSize)
    {
        GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = new Vector3(x,y,z) * cellSize + new Vector3(cellSize, cellSize, cellSize) * 0.5f;
        gameObject.GetComponent<TextMesh>().text = gameObject.transform.position.ToString(); 
        return gameObject.GetComponent<TextMesh>();
    }
}