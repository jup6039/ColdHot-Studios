﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprayable : MonoBehaviour
{
    InteractableObject thisInteract;
    // Start is called before the first frame update
    void Awake()
    {
        thisInteract = this.gameObject.GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method that gets called when shot
    public void Shot()
    {
        thisInteract.SetInteraction(true);
    }
}
