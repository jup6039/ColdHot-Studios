using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    // Variables
    public enum type { gold, steel, bronze, brass, copper, iron}

    public type matType; // type of the material
    public List<string> effect = new List<string>(); // list of material's effects

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Materials(type _type)
    {
        matType = _type;
    }
}
