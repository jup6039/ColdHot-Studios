﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Player"; // the tagged object that can trigger the event
    [SerializeField] private GameObject triggeredWall; // the object (wall) that will be activated upon event trigger
    [SerializeField] private List<GameObject> otherTriggers = new List<GameObject>(); // a list of other objects that can be activated by the event
    [SerializeField] private bool limitedTriggers = false;
    [SerializeField] private int numtriggersAllowed;

    private bool isWall;
    private List<bool> isTriggers = new List<bool>();
    private int numTriggers;

    // Start is called before the first frame update
    void Start()
    {

        isWall = triggeredWall.activeSelf;

        for (int i = 0; i < otherTriggers.Count; i++)
        {
            isTriggers.Add(otherTriggers[i].activeSelf);
        }

        numTriggers = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (limitedTriggers)
        {
            if (numTriggers <= numtriggersAllowed)
            {
                // check if the triggering object holds the correct tag
                if (other.gameObject.tag == triggerTag)
                {
                    triggeredWall.SetActive(!isWall); // activate the wall

                    for (int i = 0; i < otherTriggers.Count; i++)
                    {
                        otherTriggers[i].SetActive(!isTriggers[i]);
                    }

                    numTriggers++;
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            // check if the triggering object holds the correct tag
            if (other.gameObject.tag == triggerTag)
            {
                triggeredWall.SetActive(!isWall); // activate the wall

                for (int i = 0; i < otherTriggers.Count; i++)
                {
                    otherTriggers[i].SetActive(!isTriggers[i]);
                }
            }
        }
    }
}
