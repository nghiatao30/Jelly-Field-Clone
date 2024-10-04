using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator,cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private CubeData cubeData;
    private int selectedObjectIndex = -1;

    [SerializeField] GameObject gridVisualization;
    private Renderer previewRenderer;
    [SerializeField] GridData gridData;

    private List<GameObject> placedGameObjects = new();

    private void Start()
    {
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = cubeData.objectsData.FindIndex(x => x.ID == ID);
        if(selectedObjectIndex < 0 )
        {
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
       if(inputManager.IsPointerOverUI())
        {
            return;
        }
       Vector3 mousePosition = inputManager.GetSelectedMapPosition();
       Vector3Int gridPosition = grid.WorldToCell(mousePosition);

       bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity) return;

       GameObject placedObject = inputManager.selectedGameObject;
       PlacementData interacObject = placedObject.GetComponent<PlacementData>();
       interacObject.OnDoned();
       placedObject.transform.position = grid.CellToWorld(gridPosition);
       placedGameObjects.Add(placedObject);
       cellIndicator.SetActive(false);

       inputManager.isSelect = false;
       inputManager.selectedGameObject = null;

       gridData.AddObjectAt(gridPosition, interacObject,
            cubeData.objectsData[selectedObjectIndex].Size
            );
        GameplayManager.instance.CheckCube(interacObject,
            gridData.gridSubCubes,
            gridData.subGridSizeWidth,
            gridData.subGridSizeHeight,
            gridPosition);
        if(gridData.IsFull())
        {
            GameplayManager.instance.isWinMission = false;
            GameplayManager.instance.isGameOver = true;
        }
           
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObject(gridPosition, cubeData.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {   
        if(selectedObjectIndex < 0) { return; }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
