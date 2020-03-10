using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button modButton;
    public Button craftButton;
    public Button goldButton;
    public Button ironButton;
    public GameObject player;
    public GameObject textPrompt;
    public GameObject needMorePrompt;
    public GameObject modPanel;
    public GameObject modPrefab;
    public GameObject matPanel;
    public GameObject matPrefab;
    public GameObject goldPanel;
    public GameObject ironPanel;
    public Sprite borderImage;

    private GameObject moddingMenu;
    private GameObject craftingMenu;
    private GameObject[] workbenches;
    [HideInInspector] public List<GameObject> draggables;
    private bool isPaused;
    private bool canCraft;
    private int modListPosition;
    private int matListPosition;
    private Dictionary<string, int> playerMods;
    private Dictionary<string, int> materialInventory;

    // for testing purposes only
    //public List<GameObject> mods;
    public Sprite testMod;
    public GameObject testPanel;

    // Start is called before the first frame update
    void Start()
    {
        modListPosition = 0;
        matListPosition = 0;
        Time.timeScale = 1;

        // find menu panels
        moddingMenu = GameObject.FindGameObjectWithTag("ModdingMenu");
        craftingMenu = GameObject.FindGameObjectWithTag("CraftingMenu");
        workbenches = GameObject.FindGameObjectsWithTag("Workbench");

        foreach(GameObject bench in workbenches)
        {
            Debug.Log("bench found!");
        }

        // set all panels and buttons to be invisible
        textPrompt.SetActive(false);
        needMorePrompt.SetActive(false);
        moddingMenu.SetActive(false);
        craftingMenu.SetActive(false);
        modButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        modPanel.SetActive(false);
        testPanel.SetActive(false);
        goldPanel.SetActive(false);
        ironPanel.SetActive(false);

        // bool variables
        isPaused = false;
        canCraft = false;

        // lock cursor for fps
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMods = player.GetComponent<PlayerInventory>().playerMods;
        materialInventory = player.GetComponent<PlayerInventory>().materialInventory;

        // create panel list of all available mods
        foreach (KeyValuePair<string, int> entry in playerMods)
        {
            GameObject newMod = Instantiate(modPrefab);
            newMod.transform.SetParent(modPanel.transform);

            RectTransform modTransform = newMod.GetComponent<RectTransform>();
            modTransform.anchoredPosition = new Vector2((100 * modListPosition) - 400, 0);

            draggables.Add(newMod);

            modListPosition++;
        }

        // create panel list of material inventory
        foreach (KeyValuePair<string, int> entry in materialInventory)
        {
            GameObject newMat = Instantiate(matPrefab);
            newMat.transform.SetParent(matPanel.transform);

            RectTransform matTransform = newMat.GetComponent<RectTransform>();
            matTransform.anchoredPosition = new Vector2(0, (120 * -matListPosition) + 80);

            GameObject matChild = newMat.transform.GetChild(0).gameObject;
            matChild.GetComponent<Text>().text = entry.Key + " " + entry.Value;

            matListPosition++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bench in workbenches)
        {
            if (bench.GetComponent<BoxCollider>().bounds.Contains(player.transform.position) && Time.timeScale == 1)
            {
                textPrompt.SetActive(true);
                canCraft = true;
                Debug.Log("collision");
                break;
            }
            else
            {
                textPrompt.SetActive(false);
                canCraft = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
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
                goldButton.onClick.AddListener(GoldReveal);
                ironButton.onClick.AddListener(IronReveal);
                modPanel.SetActive(true);
            }
            else if (Time.timeScale == 0 && isPaused == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("high");
                Time.timeScale = 1;
                isPaused = false;
                textPrompt.SetActive(false);
                moddingMenu.SetActive(false);
                craftingMenu.SetActive(false);
                modButton.gameObject.SetActive(false);
                craftButton.gameObject.SetActive(false);
                testPanel.SetActive(false);
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
        textPrompt.SetActive(false);
    }

    void SwitchToCraft()
    {
        Debug.Log("clicked");
        moddingMenu.SetActive(false);
        modPanel.SetActive(false);
        testPanel.SetActive(false);
        craftingMenu.SetActive(true);
        textPrompt.SetActive(false);
    }

    void GoldReveal()
    {
        goldPanel.SetActive(true);
        ironPanel.SetActive(false);
        needMorePrompt.SetActive(false);
    }

    void IronReveal()
    {
        goldPanel.SetActive(false);
        ironPanel.SetActive(true);
        needMorePrompt.SetActive(false);
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

    public void UpdateInventory()
    {
        matListPosition = 0;

        foreach (Transform child in matPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, int> entry in materialInventory)
        {
            GameObject newMat = Instantiate(matPrefab);
            newMat.transform.SetParent(matPanel.transform);

            RectTransform matTransform = newMat.GetComponent<RectTransform>();
            matTransform.anchoredPosition = new Vector2(0, (120 * -matListPosition) + 80);

            GameObject matChild = newMat.transform.GetChild(0).gameObject;
            matChild.GetComponent<Text>().text = entry.Key + " " + entry.Value;

            matListPosition++;
        }
    }
}
