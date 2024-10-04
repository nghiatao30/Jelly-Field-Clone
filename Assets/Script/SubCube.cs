using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubCube : MonoBehaviour
{
    public int width;
    public int height;
    public int id;
    public Vector3Int starPos;
    public Colour color;
    private PlacementData parentCube;
    bool canDestroy = false;

    private void Start()
    {
        parentCube = GetComponentInParent<PlacementData>(); 
    }
    public bool CheckToDissappear(SubCube[,] subCubes, int width, int height, Vector2Int pos)
    {   
        for (int i = 0; i < this.width; ++i)
        {
            for(int j = 0; j < this.height; ++j)
            {
                CheckOnPos(subCubes,width,height, pos.x + i , pos.y + j - 1);
                CheckOnPos(subCubes, width,height, pos.x + i, pos.y + j + 1);
                CheckOnPos(subCubes, width, height, pos.x + i - 1, pos.y + j);
                CheckOnPos(subCubes, width, height, pos.x + i + 1, pos.y + j);
            }
        }
        if (canDestroy)
        {
            DestroyCube();
            return true;
        }
        return false;
    }

    private void CheckOnPos(SubCube[,] subCubes, int width, int height, int posX, int posY)
    {   
        
        if (posX == width || posY == height)
        {
            return;
        }
        if (posX < 0 || posY < 0) {
            return;
        }
        if (subCubes[posX, posY] == null) return;
        if (subCubes[posX, posY].gameObject == this) {
            return;
        }
        if (IsSameColor(subCubes[posX, posY]))
        {
            Debug.Log("Work");
            PlacementData parentNeightbor = subCubes[posX, posY].parentCube;
            parentNeightbor.RemoveAndReshape(subCubes[posX, posY]);
            canDestroy = true;
        }
    }

    public bool IsSameColor(SubCube subCube)
    {   
        if (subCube == null) return false;
        if(subCube.gameObject != this.gameObject) return subCube.color == color;
        return false;

    }


    public void DestroyCube()
    {
        if(parentCube == null)
        {
            Debug.Log("No parent Cube ");
            return;
        }
    }

    public void ReshapeCube(int width, int height)
    {
        Debug.Log("Reshape to " + this.gameObject.name);
        this.width = width;
        this.height = height;
        this.transform.localScale = new Vector3(0.5f * width, 1f, 0.5f * height);
    }
}

public enum Colour{ blue, red, green, yellow, purple, black};