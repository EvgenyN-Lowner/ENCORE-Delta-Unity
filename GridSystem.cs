using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvgenyN.Utils;

public class GridSystem<TGridObject>
{
    bool showDebug = false;

    private int gridWidth;
    private int gridHeight;
    private float gridCellSize;

    private TGridObject[,] gridArray;
    private Vector3 originPos;
    private int[,] gridDataVectorX;
    private int[,] gridDataVectorZ;

    public event EventHandler<OnGridObjectChangedEventArgs> onGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    public GridSystem(int gridWidth, int gridHeight, float gridCellSize, Vector3 originPos, Func<GridSystem<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.gridCellSize = gridCellSize;
        this.originPos = originPos;

        gridArray = new TGridObject[gridWidth, gridHeight];
        gridDataVectorX = new int[gridWidth, gridHeight];
        gridDataVectorZ = new int[gridWidth, gridHeight];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
                SetData(0, 1, x, z);
                SetData(0, 2, x, z);
            }
        }

        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[gridWidth, gridHeight];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPos(0, gridHeight), GetWorldPos(gridWidth, gridHeight), Color.white, 100f);
            Debug.DrawLine(GetWorldPos(gridWidth, 0), GetWorldPos(gridWidth, gridHeight), Color.white, 100f);

            onGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * gridCellSize + originPos;
    }

    public void GetXZ(Vector3 worldPos, out float x, out float z)
    {
        x = Mathf.Round((worldPos - originPos).x / gridCellSize);
        z = Mathf.Round((worldPos - originPos).z / gridCellSize);
    }

    //ID's | Air-0 | Wall-1 | 
    public void SetData(int id, int vector, int x, int z)
    {
        if (vector == 1)
            gridDataVectorX[x, z] = id;
        if (vector == 2)
            gridDataVectorZ[x, z] = id;
    }

    public int GetData(int vector, int x, int z)
    {
        int id = 0;
        if (vector == 1)
            id = gridDataVectorX[x, z];
        if (vector == 2)
            id = gridDataVectorZ[x, z];
        return id;
    }
}
