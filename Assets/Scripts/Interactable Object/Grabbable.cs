using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    //the interactable object that tells us if this interaction can happen
    private InteractableObject interactionHolder;

    //camera object to help determin where to place things
    private Camera ourCamera;

    //rigidbody to prevent rapid acceleration
    private Rigidbody ourRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        interactionHolder = this.gameObject.GetComponent<InteractableObject>();
        ourRigidBody = this.gameObject.GetComponent<Rigidbody>();

        //destroy if not interactable
        if (interactionHolder == null)
        {
            Destroy(this);
        }


        ourCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionHolder.Interaction)
        {
            Transform camTransform = ourCamera.transform;
            Vector3 faceVector = camTransform.forward * 2;
            Vector3 newposition = camTransform.position + faceVector;
            this.gameObject.transform.position = newposition;
            this.gameObject.tag = "Held";

            ourRigidBody.velocity = new Vector3(0, 0, 0);
        }
    }
}
