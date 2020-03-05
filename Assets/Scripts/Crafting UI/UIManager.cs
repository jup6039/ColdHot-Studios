using System.Collections;
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
    public GameObject modPanelPrefab;
    public Sprite borderImage;

    private GameObject moddingMenu;
    private GameObject craftingMenu;
    private GameObject[] workbenches;
    private bool isPaused;
    private bool canCraft;

    // for testing purposes only
    //public List<GameObject> mods;
    public Sprite testMod;

    // Start is called before the first frame update
    void Start()
    {
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

        // bool variables
        isPaused = false;
        canCraft = false;

        // lock cursor for fps
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // create panel list of all available mods
        for (int i = 0; i < 20; i++)
        {
            // create panel for mod
            GameObject newPanel = new GameObject();
            newPanel.transform.SetParent(modPanel.transform);
            Image panelImage = newPanel.AddComponent<Image>();
            panelImage.sprite = borderImage;

            RectTransform panelTransform = newPanel.GetComponent<RectTransform>();
            panelTransform.anchoredPosition = new Vector2((100 * i) - 400, 0);

            //newPanel.transform.localScale += new Vector3(2f, 2f, 2f);

            // create mod
            GameObject newMod = new GameObject();
            newMod.transform.SetParent(newPanel.transform);
            Image newImage = newMod.AddComponent<Image>();
            newImage.sprite = testMod;

            newMod.AddComponent<DragDrop>();

            RectTransform modTransform = newMod.GetComponent<RectTransform>();
            //modTransform.anchoredPosition = new Vector2((100 * i) - 400, 0);
            modTransform.anchoredPosition = new Vector2(0, 0);

            newMod.transform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);

            // create another panel and set up buttons if there are more than a certain number of mods
            /*if (i % 5 == 0)
            {
                GameObject newModPanel = Instantiate(modPanelPrefab);
                newModPanel.transform.SetParent(moddingMenu.transform);
                newModPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                modPanel = newModPanel;
            }*/
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
        craftingMenu.SetActive(false);
    }

    void SwitchToCraft()
    {
        Debug.Log("clicked");
        moddingMenu.SetActive(false);
        modPanel.SetActive(false);
        craftingMenu.SetActive(true);
    }

    // whenever a new mod is created, another mod icon is added to the list
    // create new gameobject with mod image
    // look at most recent mod in list
    // set up anchored position points based on previous mod in list
    void AddToModList()
    {
        
    }
}
