using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Variables
    public List<Tool> playerTools = new List<Tool>(); // list of tools the player has
    public Dictionary<Materials, int> materialInventory = new Dictionary<Materials, int>(); // dictionary holding the materials the player has, and how much of each
    public List<Mods> playerMods = new List<Mods>(); // list of the mods the player has
    public Tool equippedTool; // reference to equipped tool (from playerTools list)

    private Materials iron;
    private Materials gold;
    private Materials copper;
    private Materials bronze;
    private Materials steel;
    private Materials brass;

    // Start is called before the first frame update
    void Start()
    {
        // initialize materials
        /*gold.matType = Materials.type.gold; // set material type
        iron.matType = Materials.type.iron;
        copper.matType = Materials.type.copper;
        bronze.matType = Materials.type.bronze;
        steel.matType = Materials.type.steel;
        brass.matType = Materials.type.brass;*/

        gold = new Materials(Materials.type.gold);
        iron = new Materials(Materials.type.iron);
        copper = new Materials(Materials.type.copper);
        bronze = new Materials(Materials.type.bronze);
        steel = new Materials(Materials.type.steel);
        brass = new Materials(Materials.type.brass);

        materialInventory.Add(gold, 2);
        Debug.Log(gold.matType);
        Debug.Log(iron.matType);
        Debug.Log(materialInventory[iron]);
        //materialInventory.Add(iron, 50);
        //materialInventory.Add(copper, 5);
        //materialInventory.Add(bronze, 7);
        //materialInventory.Add(steel, 13);
        //materialInventory.Add(brass, 23);

        Debug.Log("Count: " + materialInventory.Count);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnGUI used to print to the screen
    private void OnGUI()
    {
        GUI.Label(new Rect(0.0f, 0.0f, 1000.0f, 500.0f), "Gold: " + materialInventory[gold] + "  Iron: " + materialInventory[iron] + "  Copper: " + materialInventory[copper] + "  Bronze: " + materialInventory[bronze] + "  Steel: " + materialInventory[steel] + "  Brass: " + materialInventory[brass]);
    }
}
