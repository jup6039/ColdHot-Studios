using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // Variables
    public enum inputType { mod, material} // the input the slot can take
    public enum slotType { nozzle, loader, stock} // the type of slot it is

    private inputType type;
    private slotType subtype;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Constructor
    /// Args:
    ///     - inputType _type : assigns the type of the slot
    ///     - slotType _subtype : assigns the subtype of the slot
    /// </summary>
    public Slot(inputType _type, slotType _subtype)
    {
        type = _type;
        subtype = _subtype;
    }
}
