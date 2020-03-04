using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button modButton;
    public Button craftButton;
    //public Button modButton2;
    //public Button craftButton2;

    private GameObject moddingMenu;
    private GameObject craftingMenu;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        moddingMenu = GameObject.FindGameObjectWithTag("ModdingMenu");
        craftingMenu = GameObject.FindGameObjectWithTag("CraftingMenu");
        moddingMenu.SetActive(false);
        craftingMenu.SetActive(false);
        modButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1 && isPaused == false)
            {
                Time.timeScale = 0;
                isPaused = true;
                moddingMenu.SetActive(true);
                modButton.gameObject.SetActive(true);
                craftButton.gameObject.SetActive(true);
                modButton.onClick.AddListener(SwitchToMod);
                craftButton.onClick.AddListener(SwitchToCraft);
                //modButton2.onClick.AddListener(SwitchMenu);
                //craftButton2.onClick.AddListener(SwitchMenu);
                //moddingMenu.SetActive(true);
            }
            else if (Time.timeScale == 0 && isPaused == true)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                isPaused = false;
                moddingMenu.SetActive(false);
                craftingMenu.SetActive(false);
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
