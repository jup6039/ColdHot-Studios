using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTriggerInteract : MonoBehaviour
{
    private InteractableObject interactionHolder; //the interactable object that tells us if this interaction can happen

    [SerializeField] private string triggerTag = "Player"; // the tagged object that can trigger the event
    [SerializeField] private GameObject triggeredWall; // the object (wall) that will be activated upon event trigger
    [SerializeField] private List<GameObject> otherTriggers = new List<GameObject>(); // a list of other objects that can be activated by the event
    [SerializeField] private bool limitedTriggers = false;
    [SerializeField] private int numtriggersAllowed;

    private bool isWall;
    private List<bool> isTriggers;
    private int numTriggers;

    // Start is called before the first frame update
    void Start()
    {
        interactionHolder = this.gameObject.GetComponent<InteractableObject>(); // set the interactionHolder

        numTriggers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionHolder.Interaction)
        {
            Debug.Log("HitPipe");

            isWall = triggeredWall.activeSelf;

            Debug.Log(isWall);

            if (limitedTriggers)
            {
                // check if the triggering object holds the correct tag
                if (numTriggers < numtriggersAllowed)
                {
                    triggeredWall.SetActive(!isWall); // activate the wall

                    for (int i = 0; i < otherTriggers.Count; i++)
                    {
                        bool isTrigger = otherTriggers[i].activeSelf;
                        otherTriggers[i].SetActive(!isTrigger);
                    }

                    numTriggers++;
                }
                else
                {
                    Debug.Log(numTriggers + ": DESTROY");
                    Destroy(this);
                }
            }
            else
            {
                triggeredWall.SetActive(!isWall); // activate the wwall

                for (int i = 0; i < otherTriggers.Count; i++)
                {
                    bool isTrigger = otherTriggers[i].activeSelf;
                    otherTriggers[i].SetActive(!isTrigger);
                }

                interactionHolder.ToggleInteraction();
            }
        }
    }
}
