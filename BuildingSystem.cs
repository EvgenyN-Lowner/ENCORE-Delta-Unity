using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvgenyN.Utils;

public class BuildingSystem
{
    bool isXReversed, isZReversed;
    float xPosStart = 0f, zPosStart = 0f, xPosEnd = 0f, zPosEnd = 0f;
    public void DragBuildingSystem(GridSystem<WorldController.GridObject> grid, GameObject buildingObject, Transform parent)
    {

        if (Input.GetMouseButtonDown(0))
        {
            GetMouseXZ(grid, out float xPosS, out float zPosS);
            xPosStart = xPosS;
            zPosStart = zPosS;

        }
        if (Input.GetMouseButtonUp(0))
        {
            GetMouseXZ(grid, out float xPosE, out float zPosE);
            xPosEnd = xPosE;
            zPosEnd = zPosE;

            float xAxisCalc = xPosEnd - xPosStart;
            float zAxisCalc = zPosEnd - zPosStart;

            if (xAxisCalc < 0)
            {
                UtilsMain.GetPositiveFloat(xAxisCalc, out float x);
                xAxisCalc = x;
                isXReversed = true;
            }
            else
            {
                isXReversed = false;
            }

            if (zAxisCalc < 0)
            {
                UtilsMain.GetPositiveFloat(zAxisCalc, out float z);
                zAxisCalc = z;
                isZReversed = true;
            }
            else
            {
                isZReversed = false;
            }

            if (xAxisCalc > zAxisCalc)
            {
                for (int i = 0; i < xAxisCalc; i++)
                {
                    if (!isXReversed)
                    {
                        if (grid.GetData(1, (int)xPosStart, (int)zPosStart) == 0)
                        {
                            GameObject.Instantiate(buildingObject, new Vector3(xPosStart, 0, zPosStart), Quaternion.Euler(0, 90, 0), parent);
                            grid.SetData(1, 1, (int)xPosStart, (int)zPosStart);
                        }
                        xPosStart += 1;
                    }
                    else
                    {
                        if (grid.GetData(1, (int)xPosStart - 1, (int)zPosStart) == 0)
                        {
                            GameObject.Instantiate(buildingObject, new Vector3(xPosStart - 1, 0, zPosStart), Quaternion.Euler(0, 90, 0), parent);
                            grid.SetData(1, 1, (int)xPosStart - 1, (int)zPosStart);
                        }
                        xPosStart -= 1;
                    }
                }
            }
            else if (xAxisCalc < zAxisCalc)
            {
                for (int i = 0; i < zAxisCalc; i++)
                {
                    if (!isZReversed)
                    {
                        if (grid.GetData(2, (int)xPosStart, (int)zPosStart) == 0)
                        {
                            GameObject.Instantiate(buildingObject, new Vector3(xPosStart, 0, zPosStart), Quaternion.Euler(0, 0, 0), parent);
                            grid.SetData(1, 2, (int)xPosStart, (int)zPosStart);
                        }
                        zPosStart += 1;
                    }
                    else
                    {
                        if (grid.GetData(2, (int)xPosStart, (int)zPosStart - 1) == 0)
                        {
                            GameObject.Instantiate(buildingObject, new Vector3(xPosStart, 0, zPosStart - 1), Quaternion.Euler(0, 0, 0), parent);
                            grid.SetData(1, 2, (int)xPosStart, (int)zPosStart - 1);
                        }
                        zPosStart -= 1;
                    }
                }
            }
            else
            {
                Debug.Log("TTA");
/*                for (int i = 0; i < xAxisCalc; i++)
                {
                        if (grid.GetData(2, (int)xPosStart, (int)zPosStart) == 0)
                        {
                            GameObject.Instantiate(buildingObject, new Vector3(xPosStart, 0, zPosStart), Quaternion.Euler(0, 0, 0), parent);
                            grid.SetData(1, 2, (int)xPosStart, (int)zPosStart);
                        }
                    xPosStart += 1;
                    zPosStart += 1;
                }*/
            }
        }
    }

    public void DeleteObject(GridSystem<WorldController.GridObject> grid)
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject delObj = Mouse3D.GetGameObjectOnClick();
            float xPos = delObj.transform.position.x;
            float zPos = delObj.transform.position.z;
            int vector;

            if(delObj.transform.rotation.y > 0)
            {
                vector = 1;
            } else
            {
                vector = 2;
            }

            grid.SetData(0, vector, (int)xPos, (int)zPos);

            Object.Destroy(delObj);
        }
    }

    public void GetMouseXZ(GridSystem<WorldController.GridObject> grid, out float xPos, out float zPos)
    {
        grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out float x, out float z);

        xPos = x;
        zPos = z;
    }

    public void DrawWallBuilding(GridSystem<WorldController.GridObject> grid, GameObject obj)
    {
        GetMouseXZ(grid, out float xPos, out float zPos);
        obj.transform.position = new Vector3(xPos, 0, zPos);
    }

}
