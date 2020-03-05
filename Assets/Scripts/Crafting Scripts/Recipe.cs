using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe // : MonoBehaviour
{
    // Variables
    public enum recipeType { tool, mod}

    public Dictionary<string, int> materialCost = new Dictionary<string, int>();
    public Tool outputTool;
    public Mods outputMod;
    public string outputString;
    public int index;
    public recipeType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Recipe(Dictionary<string, int> _cost, recipeType _type, int _index, string _output)
    {
        materialCost = _cost;
        type = _type;
        index = _index;
        outputString = _output;
    }
}
