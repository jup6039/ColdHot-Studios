﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mods : Materials
{
    // Variables
    public Slot.slotType subtype;

    public Mods(Slot.slotType subtype, string _type, List<string> _effect) : base(_type, _effect)
    {
        this.subtype = subtype;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
