using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPoolManager : MonoBehaviour
{
    [SerializeField] int selectionSize;
    public List<GameObject> selections;
    int index = 0;

    private void Start()
    {
        selectionSize = selections.Count;
    }

    public void NextSelection()
    {
        if (selections.Count == 0)
        {
            return;
        }
        selections.RemoveAt(0);
        if (selections.Count != 0)
        {
            selections[0].gameObject.SetActive(true);
        }
       
    }
        
}
