using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropArea : MonoBehaviour
{
    public GameObject sceneManager;

    private List<GameObject> draggables;

    void Start()
    {
        draggables = sceneManager.GetComponent<UIManager>().draggables;
    }

    void Update()
    {
        foreach (GameObject item in draggables)
        {
            if (this.GetComponent<BoxCollider2D>().bounds.Contains(item.transform.position))
            {
                item.GetComponent<Draggable>().isinDropArea = true;
                item.GetComponent<RectTransform>().position = this.transform.position;
                item.transform.SetParent(this.transform);
            }
        }
    }
}