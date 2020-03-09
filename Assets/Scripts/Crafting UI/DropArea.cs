using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropArea : MonoBehaviour
{
    public GameObject sceneManager;

    private List<GameObject> draggables;
    //[HideInInspector] public bool isOccupied;

    void Start()
    {
        draggables = sceneManager.GetComponent<UIManager>().draggables;
        //isOccupied = false;
    }

    void Update()
    {
        foreach (GameObject item in draggables)
        {
            if (this.GetComponent<BoxCollider2D>().bounds.Contains(item.transform.position) /*&& isOccupied == false*/)
            {
                item.GetComponent<Draggable>().isInDropArea = true;
                item.GetComponent<RectTransform>().position = this.transform.position;
                //isOccupied = true;
            }
        }
    }
}