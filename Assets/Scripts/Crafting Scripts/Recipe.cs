using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    // Variables
    enum recipeType { tool, mod}

    public Dictionary<Material, int> materialCost = new Dictionary<Material, int>();
    public Tool outputTool;
    public Mods outputMod;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
