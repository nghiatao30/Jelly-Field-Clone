using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{   
    public static GameplayManager instance;
    public bool doAagain = false;
    public Stack<PlacementData> placementDataStack = new Stack<PlacementData>();
    public Stack<SubCube> cubeToExcute = new Stack<SubCube>();
    public Mission[] missionRequired;


    public bool isGameOver = false;
    public bool isWinMission = false;
    private void Start()
    {
        instance = this;
        foreach(var mission in missionRequired)
        {
            mission.textNumCube1.text = ": " + mission.numRequried;
        }
    }
    public void CheckCube(PlacementData data, SubCube[,] subCubes, int width, int height, Vector3Int position)
    {
        List<SubCube> subCubesList = data.subCubeList;
        
        foreach (var subCube in subCubesList)
        {   
            int posX = position.z * 2 + subCube.starPos.x;
            int posY = position.x * 2 + subCube.starPos.z;
            Vector2Int subPos = new Vector2Int(posX, posY);
            if(subCube.CheckToDissappear(subCubes, width, height, subPos) == true)
            {
                cubeToExcute.Push(subCube);
            }
        }
        while (cubeToExcute.Count > 0)
        {
            data.RemoveAndReshape(cubeToExcute.Pop());
        }
        ClearPlacmentDataStack();
    }

    void ClearPlacmentDataStack()
    {
        if (placementDataStack.Count == 0) return;
        Debug.Log("ThereIsSTinStack");
        while(placementDataStack.Count > 0)
        {   
            PlacementData data = placementDataStack.Pop();
            GridData gridData = data.gridData;
            CheckCube(data, data.gridData.gridSubCubes, 
               gridData.subGridSizeWidth, 
               gridData.subGridSizeHeight, 
               data.posOnGrid);
        }
    }

    public void OnValueChanged(Colour color)
    {
        foreach(var mission in missionRequired)
        {
            if(mission.color == color)
            {
                mission.numRequried -= 1;
                if(mission.numRequried == 0) mission.numRequried = 0;
                mission.textNumCube1.text = ": " + mission.numRequried;
            }
        }
        CheckMission();
    }

    private void CheckMission()
    {
        foreach(var mission in missionRequired)
        {
            if(mission.numRequried != 0)
            {
                isGameOver = false;
                isWinMission = false;
                return;
            }
        }
        isGameOver = true; isWinMission = true;
    }
}

[System.Serializable]
public class Mission
{
    public int numRequried;
    public Colour color;
    public TMP_Text textNumCube1;

    public Mission(int num, Colour color)
    {
        this.numRequried = num;
        this.color = color;
    }
}

