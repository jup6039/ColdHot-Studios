using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject moddingMenu;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        moddingMenu = GameObject.FindGameObjectWithTag("ModdingMenu");
        moddingMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                moddingMenu.SetActive(true);
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                moddingMenu.SetActive(false);
            }
        }
    }
}
