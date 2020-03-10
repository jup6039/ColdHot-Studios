using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButtonUI : MonoBehaviour
{
    public int ingredientsNum;
    public string modType;
    public List<string> ingredientsNeeded;
    public List<int> amountNeeded;
    public GameObject needMorePrompt;
    public GameObject player;
    [HideInInspector] public Dictionary<string, int> needed;
    public Mods output;
    public Button handledButton;

    // Start is called before the first frame update
    void Start()
    {
        needMorePrompt.SetActive(false);
        needed = new Dictionary<string, int>();

        for (int i = 0; i < ingredientsNum; i++)
        {
            needed.Add(ingredientsNeeded[i], amountNeeded[i]);
            Debug.Log("added to dic");
        }

        output = new Mods(Slot.slotType.nozzle, modType, new List<string>());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerInventory>().CheckInventory(needed))
        {
            needMorePrompt.SetActive(false);
            handledButton.onClick.AddListener(Craft);
        }
        else
        {
            needMorePrompt.SetActive(true);
        }
    }

    void Craft()
    {
        player.GetComponent<PlayerInventory>().Craft(needed, output);
    }
}
