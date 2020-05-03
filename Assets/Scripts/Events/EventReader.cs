using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    // Variables
    [SerializeField] private float radius;

    private bool trigger;                       // boolean saying if the event is triggered
    private int numTriggers;                    // number of times the event has triggered
    private GameObject reference;               // reference to the triggering object

    // Getters
    public int NumTriggers { get { return numTriggers; } }
    public bool Trigger { get { return trigger; } }
    public GameObject Referenece { get { return reference; } }

    // Start is called before the first frame update
    void Start()
    {
        numTriggers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Magnitude(reference.transform.position - this.transform.position);
        if (distance < radius && !trigger)
        {
            trigger = true;
            numTriggers++;
        }
        else if (distance > radius && trigger)
        {
            trigger = false;
        }
    }
}
