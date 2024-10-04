using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementData : MonoBehaviour,IInteracObject
{
    public int id;

    public List<SubCube> subCubeList;

    public SelectionPoolManager selectionPool;

    public List<Vector3Int> occupiedPositions;

    public Vector3Int posOnGrid;

    public GridData gridData;

    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }

    private void Start()
    {
        selectionPool = GetComponentInParent<SelectionPoolManager>();
        gridData = FindAnyObjectByType<GridData>();
    }

    public void RemoveAndReshape(SubCube subCube)
    {
        if(!subCubeList.Contains(subCube))
        {
            return;
        }
        ReMoveSubCube(subCube);
        int numCube = subCubeList.Count;
        Debug.Log(numCube);
        if (numCube == 0 ) {

            gridData.CleanGridPos(this.posOnGrid);
        }
        else if(numCube == 1)
        {   

            SubCube subCubeRemain = subCubeList[0];
            subCubeRemain.starPos = new Vector3Int(0,0,0);
            subCubeRemain.transform.localPosition = Vector3.zero;
            ReShapeSubCube(subCubeRemain, 2, 2);
        }
        else if (numCube == 2) 
        {
            
            SubCube subCubeRemain1 = subCubeList[0];
            SubCube subCubeRemain2 = subCubeList[1];
            if (subCubeRemain1.width == 1 && subCubeRemain1.height == 2)
            {
                if (subCubeRemain2.starPos.z == 1) subCubeRemain2.starPos.z = 0;
                ReShapeSubCube(subCubeRemain2, 1, 2);
            }
            else if (subCubeRemain1.width == 2 && subCubeRemain1.height == 1)
            {
                if (subCubeRemain2.starPos.x == 1) subCubeRemain2.starPos.x = 0;
                ReShapeSubCube(subCubeRemain2, 2, 1);
            }
            else if (subCubeRemain2.width == 2 && subCubeRemain2.height == 1)
            {
                if (subCubeRemain1.starPos.x == 1) subCubeRemain1.starPos.x = 0;
                ReShapeSubCube(subCubeRemain1, 2, 1);
            }
            else if (subCubeRemain2.width == 1 && subCubeRemain2.height == 2)
            {
                if (subCubeRemain1.starPos.z == 1) subCubeRemain1.starPos.z = 0;
                ReShapeSubCube(subCubeRemain1, 1, 2);
            }
            else
            {
                if (subCubeRemain1.starPos.x == subCubeRemain2.starPos.x)
                {
                    if (subCubeRemain1.starPos.x == 1)
                    {
                        subCubeRemain1.starPos.x = 0;
                        subCubeRemain2.starPos.x = 0;
                    }
                    ReShapeSubCube(subCubeRemain2, 2, 1);
                    ReShapeSubCube(subCubeRemain1, 2, 1);
                }
                else if (subCubeRemain1.starPos.z == subCubeRemain2.starPos.z)
                {
                    if (subCubeRemain1.starPos.z == 1)
                    {
                        subCubeRemain1.starPos.z = 0;
                        subCubeRemain2.starPos.z = 0;
                    }
                    ReShapeSubCube(subCubeRemain2, 1, 2);
                    ReShapeSubCube(subCubeRemain1, 1, 2);
                }
            }

        }
        GameplayManager.instance.placementDataStack.Push(this);
    }
    private void ReMoveSubCube(SubCube subCube)
    {   
        Debug.Log("remove " +  subCube.gameObject.name);
        subCubeList.Remove(subCube);
        gridData.RemoveOneSubCubeToSubGrid(posOnGrid, this, subCube);
        GameplayManager.instance.OnValueChanged(subCube.color);
        Destroy(subCube.gameObject);
    }
    private void ReShapeSubCube(SubCube subCube, int width, int height)
    {   
        subCube.ReshapeCube(width, height);
        gridData.ReAssignToSubGrid(posOnGrid, this, subCube);

    }
    public void OnClicked()
    {   
        PlacementSystem inputManager = FindAnyObjectByType<PlacementSystem>();
        inputManager.StartPlacement(id);
    }

    public void OnDoned()
    {
        gameObject.layer = default;
        if (selectionPool == null) return;
        selectionPool.NextSelection();
    }
}
