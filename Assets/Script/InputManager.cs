using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    private Vector3 lastPosition;

    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] private LayerMask pickupLayermask;
    public event Action OnClicked, OnExit;
    public bool isSelect = false;
    public GameObject selectedGameObject = null;
    [SerializeField] private Grid grid;
    RaycastHit hit;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(isSelect)
            {
                OnClicked?.Invoke();
            }
            else
            {
                OnClickedObject();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnExit?.Invoke();
        }
        if(selectedGameObject != null)
        {
            Vector3 mousePosition = GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            Vector3 pos = grid.CellToWorld(gridPosition);
            selectedGameObject.transform.position = new Vector3(pos.x,2, pos.z);
        }
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    
    public Vector3 GetSelectedMapPosition()
    {       
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray  = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    public void OnClickedObject()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out hit, 100, pickupLayermask))
        {
            IInteracObject interacObject = hit.transform.GetComponent<IInteracObject>();
            Debug.Log(hit.transform.name);
            if (interacObject != null)
            {
                selectedGameObject = hit.transform.gameObject;
                interacObject.OnClicked();
                isSelect = true;
            }

        }
    }
}
