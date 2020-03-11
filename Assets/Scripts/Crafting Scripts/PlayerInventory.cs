using System; // for Enum
using System.Linq; // for First and Last of dictionary
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //public enum inventoryType { material, mod, tool} // passed in to methods to perform various actions. DELETE if decide to make separate functions

    // VARIABLES
    public List<int> randomList = new List<int>();
    public List<Recipe> recipeList = new List<Recipe>(); // list of recipes (initialized in load.cs)
    public List<Tool> playerTools = new List<Tool>(); // list of tools the player has
    public Dictionary<string, int> materialInventory = new Dictionary<string, int>(); // dictionary holding the materials the player has, and how much of each
    public Dictionary<string, int> playerMods = new Dictionary<string, int>();
    //public List<Mods> playerMods = new List<Mods>(); // list of the mods the player has

    public GameObject sceneManager;

    [SerializeField] private Tool equippedTool; // reference to equipped tool (from playerTools list)

    // TEST VARIABLES: DELETE
    int testCraft;

    // Start is called before the first frame update
    void Start()
    {
        testCraft = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Test Resource Adding
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddToInventory("gold", 100);
            AddToInventory("nozzleBase", 1);
            sceneManager.GetComponent<UIManager>().UpdateInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddToInventory("iron", 200);
            sceneManager.GetComponent<UIManager>().UpdateInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddToInventory("brass", 100);
            sceneManager.GetComponent<UIManager>().UpdateInventory();
        }*/

        /*if (Input.GetKeyDown(KeyCode.M))
        {
            if (testCraft % 2 == 0)
            {
                Dictionary<string, int> inputs = new Dictionary<string, int>();
                inputs.Add("gold", 10);
                inputs.Add("iron", 20);
                inputs.Add("brass", 20);

                Mods newMod = new Mods(Slot.slotType.nozzle, "gold", new List<string>());

                Craft(inputs, newMod);
            }
            else
            {
                Dictionary<string, int> inputs = new Dictionary<string, int>();
                inputs.Add("gold", 3);
                inputs.Add("iron", 30);
                inputs.Add("brass", 5);

                Mods newMod = new Mods(Slot.slotType.nozzle, "iron", new List<string>());

                Craft(inputs, newMod);
            }
            testCraft++;
            sceneManager.GetComponent<UIManager>().AddToModList();
        }*/

        // on press of key P
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerMods.Count == 0 || playerMods.First().Value == 0) // make sure the player has mods/the "first" mod has more than 1
            {
                Debug.Log("no valid mod");
            }
            else
            {
                KeyValuePair<string, int> modComponents = playerMods.First(); // get first mod of playerMods inventory
                string modkey = modComponents.Key;

                playerMods[modkey] -= 1; // remove a mod from the player's inventory

                Mods newMod = TranslateModKey(modkey); // create a new mod

                Debug.Log(equippedTool.FillSlot(newMod)); // fill the slot in the gun with the mod
            }
        }

        // Call for pickup
    }

    // OnGUI used to print to the screen
    private void OnGUI()
    {
        // Output materials the player has to screen
        string matOutput = "";

        foreach(string key in materialInventory.Keys)
        {
            matOutput += key + ": " + materialInventory[key] + "  ";
        }

        // Output mods to the screen
        string modOutput = "";

        foreach(string mod in playerMods.Keys)
        {
            //modOutput += mod.TypeGetter + " " + mod.subtype + " "; for list version
            modOutput += mod + playerMods[mod] + "  ";
        }

        GUI.Label(new Rect(0.0f, 0.0f, 1000.0f, 50.0f), matOutput);
        GUI.Label(new Rect(0.0f, 50.0f, 1000.0f, 50.0f), modOutput);
    }

    //  METHODS


    // ---craft method
    // take in list of inputs and an output
    // call check method with list
    // remove thyat from your inventory
    // add output to your inventory

    /// <summary>
    /// Craft turns a given amount of materials into a mod, which is added to the player's inventory
    /// </summary>
    /// <param name="inputs">A dictionary which holds the material/s required, and the amount of each</param>
    /// <param name="output">The mod that is added to the player's inventory as a result</param>
    public void Craft(Dictionary<string, int> inputs, Mods output)
    {
        // check if all ingredients in the input dic are in your inventory
        bool hasMat = CheckInventory(inputs);

        // if the player has all of the necessary ingredients
        if (hasMat)
        {
            // remove the given amount of materials from the player inventory
            foreach(KeyValuePair<string, int> input in inputs)
            {
                materialInventory[input.Key] -= input.Value;
            }

            AddToInventory(output.modKey);

            // UI stuff
            sceneManager.GetComponent<UIManager>().AddToModList();
            sceneManager.GetComponent<UIManager>().UpdateInventory();

            //playerMods.Add(output); for list version
        }
        else
        {
            Debug.Log("You do not have the necessary materials");
        }
    }
    /// <summary>
    /// Overload of Craft turns a given amount of materials into a tool, which is added to the player's inventory
    /// </summary>
    /// <param name="inputs">A dictionary which holds the material/s required, and the amount of each</param>
    /// <param name="output">The tool that is added to the player's inventory</param>
    public void Craft(Dictionary<string, int> inputs, Tool output)
    {
        // check if all ingredients in the input dic are in your inventory
        bool hasMat = CheckInventory(inputs);

        bool hasTool = CheckInventory(output);

        // if the player has all of the necessary ingredients
        if (hasMat && !hasTool)
        {
            // remove the given amount of materials from the player inventory
            foreach (KeyValuePair<string, int> input in inputs)
            {
                materialInventory[input.Key] -= input.Value;
            }

            playerTools.Add(output);

            // UI stuff
            sceneManager.GetComponent<UIManager>().UpdateInventory();
        }
        else if (hasTool)
        {
            Debug.Log("You already have this tool");
        }
        else
        {
            Debug.Log("You do not have the necessary materials");
        }
    }


    // ---check method
    // takes in a list
    // makes sure you have everything in that list in your inventory

    /// <summary>
    /// CheckInventory checks the player's material inventory to make sure they have the required amount of a given object or objects
    /// </summary>
    /// <param name="needed">A dictionary holding the types of objects needed as strings, and how many of each as integers</param>
    /// <returns>True if the given amount of the given type is in the given inventory</returns>
    public bool CheckInventory(Dictionary<string, int> needed)
    {
        foreach(KeyValuePair<string, int> input in needed)
        {
            // check if the material is in your inventory
            if (!materialInventory.ContainsKey(input.Key))
            {
                return false;
            }

            // check if you have enough of the material
            if (materialInventory[input.Key] < input.Value)
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Overload 1 of CheckInventory checks the player's mod inventory to make sure they have the given mod
    /// </summary>
    /// <param name="_mod">The mod needed</param>
    /// <returns>True if the given mod is in the playerMods list</returns>
    public bool CheckInventory(Mods _mod)
    {
        // check if the mod is in your inventory
        if (!playerMods.ContainsKey(_mod.modKey))
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Overload 2 of CheckInventory checks the player's mod inventory to make sure they have the given mod
    /// </summary>
    /// <param name="_mod">The string modkey of the mod needed</param>
    /// <returns>True if the given mod is in the playerMods list</returns>
    public bool CheckInventory(string _mod)
    {
        // check if the mod is in your inventory
        if (!playerMods.ContainsKey(_mod))
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Overload 3 of CheckInventory checks the player's tool inventory to make sure they have the given tool
    /// </summary>
    /// <param name="_tool">The tool needed</param>
    /// <returns>True if the given tool is in the playerTools list</returns>
    public bool CheckInventory(Tool _tool)
    {
        // check if the tool is in your inventory
        if (!playerTools.Contains(_tool))
        {
            return false;
        }

        return true;
    }


    // ---add to inventory method
    // takes in a pickup object (string) and an amount
    // checks the pickup's type string
    // checks the dictionary for that type
    // adds that amount

    /// <summary>
    /// AddToInventory takes a material and adds a certain amount of it to the materialInventory dictionary
    /// </summary>
    /// <param name="_type"> The type of object being added to the inventory dictionary</param>
    /// <param name="_amount">The number of materials of the given type added</param>
    public void AddToInventory(string _type, int _amount)
    {
        // if the material is already in the inventory
        if (materialInventory.ContainsKey(_type))
        {
            materialInventory[_type] += _amount;
        }
        // if not, add the material to the inventory
        else
        {
            materialInventory.Add(_type, _amount);
        }

        //Debug.Log(_type + " in inventory: " + materialInventory[_type]);
    }
    /// <summary>
    /// Overload 1 of AddToInventory adds the given mod to the player's mod inventory, if they do not already have it
    /// </summary>
    /// <param name="_mod">The mod added to the inventory</param>
    public void AddToInventory(string _modKey)
    {
        // check to see if the mod is in the dic already 
        if (playerMods.ContainsKey(_modKey))
        {
            playerMods[_modKey] += 1;
        }
        
        // if not, add it to the dic
        else
        {
            playerMods.Add(_modKey, 1);
            //sceneManager.GetComponent<UIManager>().AddToModList();
        }
    }
    /// <summary>
    /// Overload 2 of AddToInventory adds the given tool to the player's tool inventory, if they do not already have it
    /// </summary>
    /// <param name="_tool">The tool added to the inventory</param>
    public void AddToInventory(Tool _tool)
    {
        // check to see if the tool is in the list already (?)

        // if not, add it to the list
    }

    /// <summary>
    /// Generates a mod using a modkey
    /// </summary>
    /// <param name="modkey">the mod key used to generate the mod</param>
    /// <returns></returns>
    public Mods TranslateModKey(string modkey)
    {
        Mods newMod;

        string[] modComponents = modkey.Split(',');

        string newType = modComponents[0];

        Slot.slotType newSubtype;
        newSubtype = (Slot.slotType)Enum.Parse(typeof(Slot.slotType), modComponents[1]);

        newMod = new Mods(newSubtype, newType, new List<string>());

        return newMod;
    }

    /*/// <summary>
    /// Generate a string key for a mod to add to the inventory put
    /// </summary>
    /// <param name="_mod"></param>
    /// <returns></returns>
    string GenModKey(Mods _mod)
    {
        string modKey = _mod.TypeGetter + " " + _mod.subtype;
        return modKey;
    }*/
}
