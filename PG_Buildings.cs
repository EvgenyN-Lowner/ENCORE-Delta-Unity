using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using System.IO;

public class PG_Buildings : MonoBehaviour
{
    private const int pg = 3;
    private const int road = 27;

    public List<PG_Preset> listPG = new List<PG_Preset>();
    public GameObject roadPrefab;
    public Vector3 pos;

    void Awake()
    {
        Transform parent = GameObject.Find("Procedural Generator").transform;

        MeshCombiner meshCombiner = gameObject.AddComponent<MeshCombiner>();

        GenerateWorld(108, 108, pos, parent);

        meshCombiner.CreateMultiMaterialMesh = true;
        meshCombiner.DestroyCombinedChildren = true;
        meshCombiner.CombineMeshes(false);
    }

    void GenerateWorld(int width, int height, Vector3 zeroPos, Transform parent)
    {
        int lastX = 0;
        int randWidth = 0;
        int randHeight = 0;

        for (int i = 0; i < width / road; i++)
        {
            for (lastX = 0; lastX < width / road; lastX = lastX + randWidth + 1)
            {
                randWidth = Random.Range(1, 5); //3
                randHeight = Random.Range(1, 1);
                if (width / road - lastX - 1 <= 3)
                {
                    Debug.LogError("ERW");
                    randWidth = randWidth + 1;
                }
                if (width / road - lastX - 1 < randWidth + 1)
                {
                    randWidth = width / road - lastX - 1;
                    Debug.LogError("ERW2");
                }
                if (width / road - (lastX + randWidth + 1) <= 2) randWidth = randWidth + width / road - (lastX + randWidth + 1);
                Debug.Log("randWidth: " + randWidth + " randHeight: " + randHeight);
                GenerateBlock(randWidth, randHeight, new Vector3(zeroPos.x + road * lastX, zeroPos.y, zeroPos.z + i * road), true, true, parent);
                Debug.Log("lastX: " + lastX + " V3: " + new Vector3(zeroPos.x + road * lastX, zeroPos.y, zeroPos.z));
            }
            Debug.Log(zeroPos.x + road * lastX);
        }
    }

    void GenerateBlock(int width, int height, Vector3 zeroPos, bool S, bool W, Transform parent)
    {
        int actWidth = 2 + width;
        int actHeight = 2 + height;

        if (S == true)
            for (int i = 1; i < actWidth; i++)
            {
                GameObject.Instantiate(roadPrefab, new Vector3(zeroPos.x + i * road, zeroPos.y, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            }
        if (W == true)
            for (int i = 0; i < actWidth - 1; i++)
            {
                GameObject.Instantiate(roadPrefab, new Vector3(zeroPos.x + i * road, zeroPos.y, zeroPos.z + actHeight * road - road), Quaternion.Euler(0, 0, 0), parent);
            }
        for (int i = 0; i < actHeight - 1; i++)
        {
            GameObject.Instantiate(roadPrefab, new Vector3(zeroPos.x, zeroPos.y, zeroPos.z + i * road), Quaternion.Euler(0, 0, 0), parent);
        }
        for (int i = 1; i < actHeight; i++)
        {
            GameObject.Instantiate(roadPrefab, new Vector3(zeroPos.x + actWidth * road - road, zeroPos.y, zeroPos.z + i * road), Quaternion.Euler(0, 0, 0), parent);
        }

        GenerateRectPG(width * road / 3, height * road / 3, new Vector3(zeroPos.x + road, zeroPos.y, zeroPos.z + road), parent);
    }

    void GenerateRectPG(int width, int height, Vector3 zeroPos, Transform parent)
    {
        float cutter;
        int widthCalc = width;
        int heightCalc = height;
        int lastX = 0;
        int lastZ = 0;
        bool isGen = false;

        if (width > height || width == height)
        {
            cutter = height / 2;
            //bot
            while (widthCalc > 0 && isGen == false)
            {
                int buildingWidth = Random.Range(4, 10);
                int buildingHeight = (int)cutter;
                widthCalc = widthCalc - buildingWidth;
                if (widthCalc < 5)
                {
                    buildingWidth = buildingWidth + widthCalc;
                    isGen = true;
                }
                int randomID = Random.Range(0, listPG.Count);
                GenerateBuilding(randomID, new Vector3(zeroPos.x + lastX * pg, zeroPos.y, zeroPos.z), buildingWidth, buildingHeight, Random.Range(listPG[randomID].minFloors, listPG[randomID].maxFloors), new bool[] { true, true, true, true }, parent);
                lastX = lastX + buildingWidth;
            }
            isGen = false;
            widthCalc = width;
            lastX = 0;
            //top
            while (widthCalc > 0 && isGen == false)
            {
                int buildingWidth = Random.Range(4, 10);
                int buildingHeight = (int)cutter;
                int heightCA = (int)cutter - buildingHeight;
                widthCalc = widthCalc - buildingWidth;
                if (widthCalc < 5)
                {
                    buildingWidth = buildingWidth + widthCalc;
                    isGen = true;
                }
                int randomID = Random.Range(0, listPG.Count);
                GenerateBuilding(randomID, new Vector3(zeroPos.x + lastX * pg, zeroPos.y, zeroPos.z + cutter * pg + heightCA * pg), buildingWidth, buildingHeight, Random.Range(listPG[randomID].minFloors, listPG[randomID].maxFloors), new bool[] { true, true, true, true }, parent);
                lastX = lastX + buildingWidth;
            }
        }
        else if (width < height)
        {
            cutter = width / 2;
            //left
            while (heightCalc > 0 && isGen == false)
            {
                int buildingWidth = (int)cutter;
                int buildingHeight = Random.Range(4, 10);
                heightCalc = heightCalc - buildingHeight;
                if (heightCalc < 5)
                {
                    buildingHeight = buildingHeight + heightCalc;
                    isGen = true;
                }
                int randomID = Random.Range(0, listPG.Count);
                GenerateBuilding(randomID, new Vector3(zeroPos.x, zeroPos.y, zeroPos.z + lastZ * pg), buildingWidth, buildingHeight, Random.Range(listPG[randomID].minFloors, listPG[randomID].maxFloors), new bool[] { true, true, true, true }, parent);
                lastZ = lastZ + buildingHeight;
            }
            isGen = false;
            heightCalc = height;
            lastZ = 0;
            //right
            while (heightCalc > 0 && isGen == false)
            {
                int buildingWidth = (int)cutter;
                int buildingHeight = Random.Range(4, 10);
                int widthCA = (int)cutter - buildingWidth;
                heightCalc = heightCalc - buildingHeight;
                if (heightCalc < 5)
                {
                    buildingHeight = buildingHeight + heightCalc;
                    isGen = true;
                }
                int randomID = Random.Range(0, listPG.Count);
                GenerateBuilding(randomID, new Vector3(zeroPos.x + cutter * pg + widthCA * pg, zeroPos.y, zeroPos.z + lastZ * pg), buildingWidth, buildingHeight, Random.Range(listPG[randomID].minFloors, listPG[randomID].maxFloors), new bool[] { true, true, true, true }, parent);
                lastZ = lastZ + buildingHeight;
            }
        }
        else Debug.LogError("GenerateTectPG Error | WIDTH HEIGHT");
    }

    void GenerateBuilding(int typeID, Vector3 zeroPos, int widthX, int lenghtZ, int heightY, bool[] NESW, Transform parent)
    {
        //TYPES_ID | 0 - Skyscraper_1 | 
        switch (typeID)
        {
            case 0:
                GenerateBottom(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                GenerateFloors(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                GenerateTop(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                GenerateRoof(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                break;
            case 1:
                GenerateBottom(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                GenerateFloors(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                GenerateTop(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                GenerateRoof(typeID, zeroPos, widthX, lenghtZ, heightY, NESW, parent);
                break;

            default:
                Debug.LogError("Invalid TYPE_ID");
                break;
        }

    }

    void GenerateBottom(int typeID, Vector3 zeroPos, int widthX, int lenghtZ, int heightY, bool[] NESW, Transform parent)
    {
        //Getting listPG with ID
        int listID = 0;
        for (int i = 0; i < listPG.Count; i++)
        {
            if (listPG[i].typeID == typeID)
            {
                listID = i;
                break;
            }
        }

        //Edges
        if (NESW[3] == true)
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x, 0, zeroPos.z), Quaternion.Euler(0, 90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 90, 0), parent);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x, 0, zeroPos.z), Quaternion.Euler(0, 90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 90, 0), parent);
        }
        if (NESW[0] == true)
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        if (NESW[1] == true)
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, -90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);

            Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z), Quaternion.Euler(0, -90, 0), parent);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, -90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z), Quaternion.Euler(0, -90, 0), parent);
        }
        if (NESW[2] == true)
        {
            Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x, 0, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            var t = Instantiate(listPG[listID].MatchTypeBottomEdge(listID)[Random.Range(0, listPG[listID].bottomEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x, 0, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
        }

        //Inner
        if (NESW[3] == true)
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeBottom(listID)[Random.Range(0, listPG[listID].bottomPrefabs.Length)], new Vector3(zeroPos.x, 0, zeroPos.z + pg * i), Quaternion.Euler(0, 90, 0), parent);
        }
        else
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x, 0, zeroPos.z + pg * i), Quaternion.Euler(0, 90, 0), parent);
        }
        if (NESW[0] == true)
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeBottom(listID)[Random.Range(0, listPG[listID].bottomPrefabs.Length)], new Vector3(zeroPos.x + pg * i, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        else
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x + pg * i, 0, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        if (NESW[1] == true)
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeBottom(listID)[Random.Range(0, listPG[listID].bottomPrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z + lenghtZ * pg - pg * i), Quaternion.Euler(0, -90, 0), parent);
        }
        else
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x + widthX * pg, 0, zeroPos.z + lenghtZ * pg - pg * i), Quaternion.Euler(0, -90, 0), parent);
        }
        if (NESW[2] == true)
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeBottom(listID)[Random.Range(0, listPG[listID].bottomPrefabs.Length)], new Vector3(zeroPos.x - pg + pg * i, 0, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
        }
        else
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeBottomNull(listID), new Vector3(zeroPos.x - pg + pg * i, 0, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
        }
    }

    void GenerateFloors(int typeID, Vector3 zeroPos, int widthX, int lenghtZ, int heightY, bool[] NESW, Transform parent)
    {
        //Getting listPG with ID
        int listID = 0;
        for (int i = 0; i < listPG.Count; i++)
        {
            if (listPG[i].typeID == typeID)
            {
                listID = i;
                break;
            }
        }

        for (int j = 1; j < heightY - 1; j++)
        {
            int floorHeight = j * 3;


            //Edges
            if (NESW[3] == true)
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x, floorHeight, zeroPos.z), Quaternion.Euler(0, 90, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 90, 0), parent);
            }
            else
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x, floorHeight, zeroPos.z), Quaternion.Euler(0, 90, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 90, 0), parent);
            }
            if (NESW[0] == true)
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            }
            else
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            }
            if (NESW[1] == true)
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, -90, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z), Quaternion.Euler(0, -90, 0), parent);
            }
            else
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, -90, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z), Quaternion.Euler(0, -90, 0), parent);
            }
            if (NESW[2] == true)
            {
                Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x, floorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
                var t = Instantiate(listPG[listID].MatchTypeFloorEdge(listID)[Random.Range(0, listPG[listID].floorEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                var t = Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x, floorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
                t.transform.localScale = new Vector3(-1, 1, 1);
                Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            }

            //Inner
            if (NESW[3] == true)
            {
                for (int i = 2; i < lenghtZ; i++)
                    Instantiate(listPG[listID].MatchTypeFloor(listID)[Random.Range(0, listPG[listID].floorPrefabs.Length)], new Vector3(zeroPos.x, floorHeight, zeroPos.z + pg * i), Quaternion.Euler(0, 90, 0), parent);
            }
            else
            {
                for (int i = 2; i < lenghtZ; i++)
                    Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x, floorHeight, zeroPos.z + pg * i), Quaternion.Euler(0, 90, 0), parent);
            }
            if (NESW[0] == true)
            {
                for (int i = 2; i < widthX; i++)
                    Instantiate(listPG[listID].MatchTypeFloor(listID)[Random.Range(0, listPG[listID].floorPrefabs.Length)], new Vector3(zeroPos.x + pg * i, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            }
            else
            {
                for (int i = 2; i < widthX; i++)
                    Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x + pg * i, floorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            }
            if (NESW[1] == true)
            {
                for (int i = 2; i < lenghtZ; i++)
                    Instantiate(listPG[listID].MatchTypeFloor(listID)[Random.Range(0, listPG[listID].floorPrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z + lenghtZ * pg - pg * i), Quaternion.Euler(0, -90, 0), parent);
            }
            else
            {
                for (int i = 2; i < lenghtZ; i++)
                    Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x + widthX * pg, floorHeight, zeroPos.z + lenghtZ * pg - pg * i), Quaternion.Euler(0, -90, 0), parent);
            }
            if (NESW[2] == true)
            {
                for (int i = 2; i < widthX; i++)
                    Instantiate(listPG[listID].MatchTypeFloor(listID)[Random.Range(0, listPG[listID].floorPrefabs.Length)], new Vector3(zeroPos.x - pg + pg * i, floorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            }
            else
            {
                for (int i = 2; i < widthX; i++)
                    Instantiate(listPG[listID].MatchTypeFloorNull(listID), new Vector3(zeroPos.x - pg + pg * i, floorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            }
        }
    }

    void GenerateTop(int typeID, Vector3 zeroPos, int widthX, int lenghtZ, int heightY, bool[] NESW, Transform parent)
    {
        //Getting listPG with ID
        int listID = 0;
        for (int i = 0; i < listPG.Count; i++)
        {
            if (listPG[i].typeID == typeID)
            {
                listID = i;
                break;
            }
        }

        int topfloorHeight = heightY * pg - pg;

        //Edges
        if (NESW[3] == true)
        {
            var t = Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 90, 0), parent);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 90, 0), parent);
        }
        if (NESW[0] == true)
        {
            var t = Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        if (NESW[1] == true)
        {
            var t = Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, -90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z), Quaternion.Euler(0, -90, 0), parent);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, -90, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z), Quaternion.Euler(0, -90, 0), parent);
        }
        if (NESW[2] == true)
        {
            Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            var t = Instantiate(listPG[listID].MatchTypeTopEdge(listID)[Random.Range(0, listPG[listID].topEdgePrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            var t = Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
            t.transform.localScale = new Vector3(-1, 1, 1);
            Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
        }

        //Inner
        if (NESW[3] == true)
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeTop(listID)[Random.Range(0, listPG[listID].topPrefabs.Length)], new Vector3(zeroPos.x, topfloorHeight, zeroPos.z + pg * i), Quaternion.Euler(0, 90, 0), parent);
        }
        else
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x, topfloorHeight, zeroPos.z + pg * i), Quaternion.Euler(0, 90, 0), parent);
        }
        if (NESW[0] == true)
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeTop(listID)[Random.Range(0, listPG[listID].topPrefabs.Length)], new Vector3(zeroPos.x + pg * i, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        else
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x + pg * i, topfloorHeight, zeroPos.z + lenghtZ * pg), Quaternion.Euler(0, 180, 0), parent);
        }
        if (NESW[1] == true)
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeTop(listID)[Random.Range(0, listPG[listID].topPrefabs.Length)], new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z + lenghtZ * pg - pg * i), Quaternion.Euler(0, -90, 0), parent);
        }
        else
        {
            for (int i = 2; i < lenghtZ; i++)
                Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x + widthX * pg, topfloorHeight, zeroPos.z + lenghtZ * pg - pg * i), Quaternion.Euler(0, -90, 0), parent);
        }
        if (NESW[2] == true)
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeTop(listID)[Random.Range(0, listPG[listID].topPrefabs.Length)], new Vector3(zeroPos.x - pg + pg * i, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
        }
        else
        {
            for (int i = 2; i < widthX; i++)
                Instantiate(listPG[listID].MatchTypeTopNull(listID), new Vector3(zeroPos.x - pg + pg * i, topfloorHeight, zeroPos.z), Quaternion.Euler(0, 0, 0), parent);
        }
    }

    void GenerateRoof(int typeID, Vector3 zeroPos, int widthX, int lenghtZ, int heightY, bool[] NESW, Transform parent)
    {
        //Getting listPG with ID
        int listID = 0;
        for (int i = 0; i < listPG.Count; i++)
        {
            if (listPG[i].typeID == typeID)
            {
                listID = i;
                break;
            }
        }

        int roofHeight = heightY * 3;

        for (int i = 1; i < widthX - 1; i++)
        {
            for (int j = 1; j < lenghtZ - 1; j++)
            {
                Instantiate(listPG[listID].MatchTypeRoof(listID)[Random.Range(0, listPG[listID].roofPrefabs.Length)], new Vector3(zeroPos.x + pg * i, roofHeight, zeroPos.z + pg * j), Quaternion.Euler(0, 0, 0), parent);
            }
        }

    }
}