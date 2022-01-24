using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PG Preset",menuName = "ENCORE/Procedural Generation Preset")]
public class PG_Preset : ScriptableObject
{
    public int typeID;
    public int minFloors;
    public int maxFloors;

    public GameObject[] bottomPrefabs;
    public GameObject[] bottomEdgePrefabs;
    public GameObject bottomNull;

    public GameObject[] floorPrefabs;
    public GameObject[] floorEdgePrefabs;
    public GameObject floorNull;

    public GameObject[] topPrefabs;
    public GameObject[] topEdgePrefabs;
    public GameObject topNull;

    public GameObject[] roofPrefabs;

    public GameObject[] MatchTypeBottom(int ID)
    {
        if (typeID == ID)
            return bottomPrefabs;
        else
            return null;
    }

    public GameObject[] MatchTypeBottomEdge(int ID)
    {
        if (typeID == ID)
            return bottomEdgePrefabs;
        else
            return null;
    }

    public GameObject MatchTypeBottomNull(int ID)
    {
        if (typeID == ID)
            return bottomNull;
        else
            return null;
    }

    public GameObject[] MatchTypeFloor(int ID)
    {
        if (typeID == ID)
            return floorPrefabs;
        else
            return null;
    }

    public GameObject[] MatchTypeFloorEdge(int ID)
    {
        if (typeID == ID)
            return floorEdgePrefabs;
        else
            return null;
    }

    public GameObject MatchTypeFloorNull(int ID)
    {
        if (typeID == ID)
            return floorNull;
        else
            return null;
    }

    public GameObject[] MatchTypeTop(int ID)
    {
        if (typeID == ID)
            return topPrefabs;
        else
            return null;
    }

    public GameObject[] MatchTypeTopEdge(int ID)
    {
        if (typeID == ID)
            return topEdgePrefabs;
        else
            return null;
    }

    public GameObject MatchTypeTopNull(int ID)
    {
        if (typeID == ID)
            return topNull;
        else
            return null;
    }

    public GameObject[] MatchTypeRoof(int ID)
    {
        if (typeID == ID)
            return roofPrefabs;
        else
            return null;
    }
}
