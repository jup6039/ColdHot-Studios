using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    // Variables
    protected List<Slot> slotList = new List<Slot>();
    private bool usedThisUpdate;
    public bool UsedThisUpdate { get { return usedThisUpdate; } set { usedThisUpdate = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Use Class: Where the use of the respective tool will be defined
    /// </summary>
    public virtual void Use()
    {
        Debug.Log("Tool used");
    }

    /// <summary>
    /// AddSlot Class: Used to populate the slotList of the tool
    /// </summary>
    void AddSlot(Slot.inputType _inputType, Slot.slotType _subtype)
    {
        // Add a slot to the tool
        Slot newSlot = new Slot(_inputType, _subtype); // create a new slot object

        slotList.Add(newSlot);
    }
}
