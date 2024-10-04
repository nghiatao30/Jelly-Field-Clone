using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData: MonoBehaviour
{
    public  int gridSizeWidth;
    public  int gridSizeHeight;
    public int subGridSizeWidth;
    public int subGridSizeHeight;
    public Dictionary<Vector3Int, PlacementData> placedObjects = new();
    public SubCube[,] gridSubCubes;
    public List<PlacementData> initalPlacementData;

    private void Start()
    {
        subGridSizeHeight = gridSizeHeight * 2;
        subGridSizeWidth = gridSizeWidth * 2;
        gridSubCubes = new SubCube[subGridSizeWidth , subGridSizeHeight ];
        Debug.Log(gridSubCubes.Length);
        GridInit();
    }

    void GridInit()
    {   
        foreach (var data in initalPlacementData)
        {
            AddObjectAt(data.posOnGrid, data, Vector2Int.zero);
        }
    }
    private void AddSubCubeToGrid(Vector3Int positionOnGrid, PlacementData data)
    {   
        if(data.subCubeList == null)
        {
            Debug.Log("Sub none");
            return;
        }
        foreach (var subCube in data.subCubeList)
        {
            int posX = positionOnGrid.z * 2 + subCube.starPos.x;
            int posY = positionOnGrid.x * 2 + subCube.starPos.z;
            Debug.Log("x: " + posX + ", y: " + posY);
            for (int i = 0; i < subCube.width; ++i)
            {
                for(int j = 0; j < subCube.height; ++j)
                {
                    
                    gridSubCubes[posX + i, posY + j] = subCube;
                }
            }
        }
    }
    private void RemoveSubCubeToGrid(Vector3Int positionOnGrid, PlacementData data)
    {
        if (data.subCubeList == null)
        {
            Debug.Log("Sub none");
            return;
        }
        foreach (var subCube in data.subCubeList)
        {
            int posX = positionOnGrid.z * 2 + subCube.starPos.x;
            int posY = positionOnGrid.x * 2 + subCube.starPos.z;
            Debug.Log("x: " + (posX) + " y: " + (posY));
            for (int i = 0; i < subCube.width; ++i)
            {
                for (int j = 0; j < subCube.height; ++j)
                {

                    gridSubCubes[posX + i, posY + j] = null;
                }
            }
        }
    }

    public void RemoveOneSubCubeToSubGrid(Vector3Int positionOnGrid, PlacementData data, SubCube subCube)
    {
        if (data.subCubeList == null || data.subCubeList.Contains(subCube) == false)
        {
            Debug.Log("SubCube dont exit");
            return;
        }
        int posX = positionOnGrid.z * 2 + subCube.starPos.x;
        int posY = positionOnGrid.x * 2 + subCube.starPos.z;
        for (int i = 0; i < subCube.width; ++i)
        {
            for (int j = 0; j < subCube.height; ++j)
            {

                gridSubCubes[posX + i, posY + j] = null;
            }
        }
    }

    public void ReAssignToSubGrid(Vector3Int positionOnGrid, PlacementData data, SubCube subCube)
    {
        if (data.subCubeList == null || data.subCubeList.Contains(subCube) == false)
        {
            Debug.Log("Sub cant reassign");
            return;
        }
        int posX = positionOnGrid.z * 2 + subCube.starPos.x;
        int posY = positionOnGrid.x * 2 + subCube.starPos.z;
        for (int i = 0; i < subCube.width; ++i)
        {
            for (int j = 0; j < subCube.height; ++j)
            {

                gridSubCubes[posX + i, posY + j] = subCube;
            }
        }
    }

    public void AddObjectAt(Vector3Int gridPosition,PlacementData data, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {   
            if(pos.x < 0 || pos.z < 0 || pos.x >= gridSizeHeight || pos.z >= gridSizeWidth)
            {
                data.transform.localPosition = Vector3.zero;
                continue;
            }
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dic already contains this cell position {pos}");
            }
            else {
                placedObjects[pos] = data;
                data.posOnGrid = pos;
                AddSubCubeToGrid(pos, data);
            } 

        }

    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        //for (int x = 0; x < objectSize.x; ++x)
        //{
        //    for(int y = 0; y < objectSize.y; ++y)
        //    {
               
        //    }
        //}
        returnVal.Add(gridPosition);
        return returnVal;
    }

    public bool CanPlaceObject(Vector3Int pos, Vector2Int objectSize)
    {
        //List<Vector3Int> positionOcuppy = CalculatePositions(gridPosition, objectSize);
        //foreach(var pos in positionOcuppy)
        //{

        //}
        if (pos.x < 0 || pos.z < 0 || pos.x >= gridSizeHeight || pos.z >= gridSizeWidth)
        {
            return false;
        }
        if (placedObjects.ContainsKey(pos)) return false;
        return true;
    }

    public void CleanGridPos(Vector3Int pos)
    {
        if (!placedObjects[pos]) return;
        placedObjects.Remove(pos);

    }

    public bool IsFull()
    {
        if(placedObjects.Count == gridSizeWidth * gridSizeHeight)
        {
            return true;
        }
        return false;
    }
}

