using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Variables
    public List<Tool> playerTools = new List<Tool>(); // list of tools the player has
    public Dictionary<Material, int> materialInventory = new Dictionary<Material, int>(); // dictionary holding the materials the player has, and how much of each
    public List<Mods> playerMods = new List<Mods>(); // list of the mods the player has
    public Tool equippedTool; // reference to equipped tool (from playerTools list)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
