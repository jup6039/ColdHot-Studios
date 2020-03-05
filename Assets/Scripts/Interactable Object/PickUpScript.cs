using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    // Variables

    private InteractableObject interactionHolder; //the interactable object that tells us if this interaction can happen

    private PlayerInventory pInventoryScript; //the player's inventory script (pulled from the player object through CharacterInput reference

    [SerializeField] private string materialPickUp = "";
    [SerializeField] private int matPickUpAmount = 0;

    [SerializeField] private string modKeyPickUp = "";

    [SerializeField] private Tool toolPickUp = null;

    // Start is called before the first frame update
    void Start()
    {
        interactionHolder = this.gameObject.GetComponent<InteractableObject>(); // set the interactionHolder

        //destroy if not interactable
        if (interactionHolder == null)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionHolder.Interaction)
        {
            // set the pInventoryScript to hold the player's inventory
            CharacterInput cInputRef = interactionHolder.CharacterReference;
            pInventoryScript = cInputRef.gameObject.GetComponent<PlayerInventory>();

            if (materialPickUp != "")
            {
                pInventoryScript.AddToInventory(materialPickUp, matPickUpAmount);
            }

            if (modKeyPickUp != "")
            {
                pInventoryScript.AddToInventory(modKeyPickUp);
            }

            if (toolPickUp != null)
            {
                pInventoryScript.AddToInventory(toolPickUp);
            }

            Destroy(this.gameObject);
        }
    }
}
