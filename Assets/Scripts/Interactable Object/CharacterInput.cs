using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CharacterInput : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {


        //interact with objects controller
        if (Input.GetKeyDown(KeyCode.E))
        {

            //find any objects we are looking at
            GameObject[] lookingAt;
            lookingAt = GameObject.FindGameObjectsWithTag("mouseOver");

            //toggle their interaction
            foreach (GameObject thisObject in lookingAt)
            {
                InteractableObject thisInteraction = thisObject.GetComponent<InteractableObject>();
                thisInteraction.ToggleInteraction();
                thisInteraction.GetReference(this); // when the player interacts with an object, store a reference to player in interaction class
            }
        }



    }

   
}