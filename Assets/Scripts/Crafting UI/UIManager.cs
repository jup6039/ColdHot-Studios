﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button modButton;
    public Button craftButton;
    public GameObject player;
    public GameObject textPrompt;
    public GameObject modPanel;
    public GameObject modPrefab;
    public Sprite borderImage;

    private GameObject moddingMenu;
    private GameObject craftingMenu;
    private GameObject[] workbenches;
    [HideInInspector] public List<GameObject> draggables;
    private bool isPaused;
    private bool canCraft;
    private int modListPosition;
    private Dictionary<string, int> playerMods;

    // for testing purposes only
    //public List<GameObject> mods;
    public Sprite testMod;
    public GameObject testPanel;

    // Start is called before the first frame update
    void Start()
    {
        modListPosition = 0;
        Time.timeScale = 1;

        // find menu panels
        moddingMenu = GameObject.FindGameObjectWithTag("ModdingMenu");
        craftingMenu = GameObject.FindGameObjectWithTag("CraftingMenu");
        workbenches = GameObject.FindGameObjectsWithTag("Workbench");

        // set all panels and buttons to be invisible
        textPrompt.SetActive(false);
        moddingMenu.SetActive(false);
        craftingMenu.SetActive(false);
        modButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        modPanel.SetActive(false);
        testPanel.SetActive(false);

        // bool variables
        isPaused = false;
        canCraft = false;

        // lock cursor for fps
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMods = player.GetComponent<PlayerInventory>().playerMods;

        // create panel list of all available mods
        foreach (KeyValuePair<string, int> entry in playerMods)
        {
            GameObject newMod = Instantiate(modPrefab);
            newMod.transform.SetParent(modPanel.transform);

            RectTransform modTransform = newMod.GetComponent<RectTransform>();
            modTransform.anchoredPosition = new Vector2((100 * modListPosition) - 400, 0);

            draggables.Add(newMod);

            modListPosition++;

            // create panel for mod
            /*GameObject newPanel = new GameObject();
            newPanel.transform.SetParent(modPanel.transform);
            
            Image panelImage = newPanel.AddComponent<Image>();
            panelImage.sprite = borderImage;

            RectTransform panelTransform = newPanel.GetComponent<RectTransform>();
            panelTransform.anchoredPosition = new Vector2((100 * i) - 400, 0);

            // create mod
            GameObject newMod = new GameObject();
            newMod.transform.SetParent(newPanel.transform);
            Image newImage = newMod.AddComponent<Image>();
            newImage.sprite = testMod;
            newMod.AddComponent<Draggable>();

            RectTransform modTransform = newMod.GetComponent<RectTransform>();
            modTransform.anchoredPosition = new Vector2(0, 0);

            newMod.transform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1 && isPaused == false && canCraft == true)
            {
                // unlock cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // freeze game
                Time.timeScale = 0;

                // bool
                isPaused = true;

                // show menus and set up buttons
                moddingMenu.SetActive(true);
                modButton.gameObject.SetActive(true);
                craftButton.gameObject.SetActive(true);
                testPanel.SetActive(true);
                textPrompt.SetActive(false);
                modButton.onClick.AddListener(SwitchToMod);
                craftButton.onClick.AddListener(SwitchToCraft);
                modPanel.SetActive(true);
            }
            else if (Time.timeScale == 0 && isPaused == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("high");
                Time.timeScale = 1;
                isPaused = false;
                moddingMenu.SetActive(false);
                craftingMenu.SetActive(false);
                modButton.gameObject.SetActive(false);
                craftButton.gameObject.SetActive(false);
            }
        }

        foreach (GameObject bench in workbenches)
        {
            if (bench.GetComponent<BoxCollider>().bounds.Contains(player.transform.position))
            {
                textPrompt.SetActive(true);
                canCraft = true;
            }
            else
            {
                textPrompt.SetActive(false);
                canCraft = false;
            }
        }
    }

    void SwitchToMod()
    {
        Debug.Log("clicked");
        moddingMenu.SetActive(true);
        modPanel.SetActive(true);
        testPanel.SetActive(true);
        craftingMenu.SetActive(false);
    }

    void SwitchToCraft()
    {
        Debug.Log("clicked");
        moddingMenu.SetActive(false);
        modPanel.SetActive(false);
        testPanel.SetActive(false);
        craftingMenu.SetActive(true);
    }

    // whenever a new mod is created, another mod icon is added to the list
    // create new gameobject with mod image
    // look at most recent mod in list
    // set up anchored position points based on previous mod in list
    public void AddToModList()
    {
        GameObject newMod = Instantiate(modPrefab);
        newMod.transform.SetParent(modPanel.transform);

        RectTransform modTransform = newMod.GetComponent<RectTransform>();
        modTransform.anchoredPosition = new Vector2((100 * modListPosition) - 400, 0);

        draggables.Add(newMod);

        modListPosition++;
        Debug.Log(modListPosition);
    }
}
