using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    // Variables
    protected Dictionary<Slot, bool> slotList = new Dictionary<Slot, bool>();
    protected List<Mods> filledSlots = new List<Mods>();
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
    protected void AddSlot(Slot.inputType _inputType, Slot.slotType _subtype)
    {
        // Add a slot to the tool
        Slot newSlot = new Slot(_inputType, _subtype); // create a new slot object

        slotList.Add(newSlot, false); // add to slot list (false means empty)
    }

    /// <summary>
    /// FillSlot Class: Checks if the mod fits in a slot, and puts it there [MAYBE MOVE TO UI CODE???]
    /// </summary>
    /// <param name="_fillSlot">The mod to fill the open slot</param>
    /// <returns></returns>
    public string FillSlot(Mods _fillSlot) // CHANGE to protected?
    {
        foreach (Slot slot in slotList.Keys)
        {
            if (slot.Subtype == _fillSlot.subtype)
            {
                if (slotList[slot])
                {
                    return "filled"; // if the slot is already filled, return "filled"
                }

                slotList[slot] = true;

                filledSlots.Add(_fillSlot);

                onSlotFilled(_fillSlot);

                return "successful slotting";
            }
        }

        return "noslot"; // if there is no such slot, return "noslot"
    }

    /// <summary>
    /// Makes changes to the tool that are triggered by filling a mod
    /// </summary>
    /// <param name="_filledMod">the mod that filled</param>
    public virtual void onSlotFilled(Mods _filledMod)
    {
        
    }
}
