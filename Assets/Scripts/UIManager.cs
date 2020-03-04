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
    //public Button modButton2;
    //public Button craftButton2;

    private GameObject moddingMenu;
    private GameObject craftingMenu;
    private GameObject[] workbenches;
    private bool isPaused;
    private bool canCraft;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        moddingMenu = GameObject.FindGameObjectWithTag("ModdingMenu");
        craftingMenu = GameObject.FindGameObjectWithTag("CraftingMenu");
        workbenches = GameObject.FindGameObjectsWithTag("Workbench");
        textPrompt.SetActive(false);
        moddingMenu.SetActive(false);
        craftingMenu.SetActive(false);
        modButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        isPaused = false;
        canCraft = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1 && isPaused == false && canCraft == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                isPaused = true;
                moddingMenu.SetActive(true);
                modButton.gameObject.SetActive(true);
                craftButton.gameObject.SetActive(true);
                textPrompt.SetActive(false);
                modButton.onClick.AddListener(SwitchToMod);
                craftButton.onClick.AddListener(SwitchToCraft);
                //modButton2.onClick.AddListener(SwitchMenu);
                //craftButton2.onClick.AddListener(SwitchMenu);
                //moddingMenu.SetActive(true);
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
            /*if ((bench.transform.position.x + 5) <= player.transform.position.x || 
                (bench.transform.position.x - 5) >= player.transform.position.x || 
                (bench.transform.position.z + 5) <= player.transform.position.z || 
                (bench.transform.position.z - 5) >= player.transform.position.z)
            {
                textPrompt.SetActive(true);
            }*/
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
        craftingMenu.SetActive(false);
    }

    void SwitchToCraft()
    {
        Debug.Log("clicked");
        moddingMenu.SetActive(false);
        craftingMenu.SetActive(true);
    }
}
