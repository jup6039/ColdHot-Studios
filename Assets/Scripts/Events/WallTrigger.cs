using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Player"; // the tagged object that can trigger the event
    [SerializeField] private GameObject triggeredWall; // the object (wall) that will be activated upon event trigger
    [SerializeField] private List<GameObject> otherTriggers = new List<GameObject>(); // a list of other objects that can be activated by the event

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if the triggering object holds the correct tag
        if (other.gameObject.tag == triggerTag)
        {
            triggeredWall.SetActive(true); // activate the wall

            for (int i = 0; i < otherTriggers.Count; i++)
            {
                otherTriggers[i].SetActive(true);
            }
        }
    }
}
