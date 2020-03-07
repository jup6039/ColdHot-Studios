using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // stores a reference to the character that interacted with the object
    private CharacterInput characterReference;

    //boolean value stores whether the gameobject is in the interactable state, other scripts on the gameobject will read this value to determine how they act in the update state
    private bool interaction;

    //boolean value stores whether the gameobjects interaction has been voided and is no longer available
    private bool interactionVoided;

    //boolean value that stores whether the object is a player interactable, defaults to true, if false player cannot access with mouse and e
    [SerializeField] bool standardInteractable = true;

    //the materials object color before being highlighted
    private Color startcolor;

    //the renderer needed to alter the color of the object
    private Renderer renderer;

    public bool Interaction { get { return interaction; } }
    public bool InteractionVoided { get { return interactionVoided; } }
    public CharacterInput CharacterReference { get { return characterReference; } }

    // Start is called before the first frame update
    void Start()
    {
        interactionVoided = false;
        interaction = false;
        renderer = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //invokable method to toggle whether being interacted with
    public bool ToggleInteraction()
    {
        interaction = !interaction;

        //if interaction voided then set to false
        if (interactionVoided)
        {
            interaction = false;
        }

        return interaction;
    }

    //invokable method to set if being interacted with
    public bool SetInteraction(bool a_interaction)
    {
        //if interaction has not been voided set interaction else is false
        if (!interactionVoided)
        {
            interaction = a_interaction;
        }
        else
        {
            interaction = false;
        }
        return interaction;
    }

    //sets interactions to void, preventing the interaction from becoming true, used to represent a limited time interaction no longer being available or an item being used up.
    public void VoidInteractions()
    {
        interactionVoided = true;
    }

    // sets the character reference to an object
    public void GetReference(CharacterInput reference)
    {
        characterReference = reference;
    }

    private void OnMouseEnter()
    {
        if (standardInteractable)
        {
            startcolor = renderer.material.color;
            renderer.material.color = new Color(2, 2, 0);
            this.gameObject.tag = "mouseOver";
        }
    }

    private void OnMouseExit()
    {
        if (standardInteractable)
        {
            renderer.material.color = startcolor;
            if (this.gameObject.tag == "mouseOver")
            {
                this.gameObject.tag = "Untagged";
            }
        }
    }
}
