using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private string newScene;

    private InteractableObject interactionHolder; //the interactable object that tells us if this interaction can happen

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
        if(interactionHolder.Interaction)
        {
            SceneManager.LoadScene(newScene);
        }
    }

    private void OnGUI()
    {
        if (this.gameObject.tag == "mouseOver")
        {
            Vector2 center = new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);

            GUI.Label(new Rect(center.x, center.y, 1000.0f, 50.0f), "Press 'E' to transition to " + newScene);
        }
    }
}
