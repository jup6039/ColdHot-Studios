using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpray : MonoBehaviour
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
        if(thisInteract.Interaction)
        {
            Vector3 newPos = this.gameObject.transform.position;
            newPos.y++;
            this.gameObject.transform.position = newPos;
            thisInteract.ToggleInteraction();
        }

    }
}
