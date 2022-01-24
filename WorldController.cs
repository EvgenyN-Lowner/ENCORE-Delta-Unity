using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    GridSystem<GridObject> grid;
    BuildingSystem buildS;

    public GameObject deltatest;
    public GameObject greedOBJ;

    bool isDragBuilding = false;

    private void Awake()
    {
        greedOBJ = Instantiate(greedOBJ);
        int gridWidth = 100;
        int gridHeight = 100;
        float cellSize = 1f;

        grid = new GridSystem<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridSystem<GridObject> g, int x, int z) => new GridObject (g, x, z));
        buildS = new BuildingSystem();
    }

    public class GridObject
    {
        private GridSystem<GridObject> grid;
        private int x;
        private int z;

        public GridObject(GridSystem<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return x + ", " + z;
        }
    }

    private void Update()
    {
        if (isDragBuilding)
        {
            buildS.DrawWallBuilding(grid, greedOBJ);
            buildS.DragBuildingSystem(grid, deltatest, GameObject.Find("BuildingController").transform);
            buildS.DeleteObject(grid);
        }
    }

}
