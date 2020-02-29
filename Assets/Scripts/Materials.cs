using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    // Variables
    //public enum type { gold, steel, bronze, brass, copper, iron}
    protected string type; // type of the material
    public List<string> effect = new List<string>(); // list of material's effects

    // Getters and Setters
    public string TypeGetter { get { return type; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Materials(string _type, List<string> _effect)
    {
        type = _type;
        effect = _effect;
    }
}
