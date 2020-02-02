using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public Canvas pauseMenu;
    public GameObject pC;
    public bool pauseActive;

    void Awake()
    {
        pauseActive = false;
        
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("TechPlayground");
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void WakeUpMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("L key was pressed.");

            if (pauseActive == true)
            {
                pC.SetActive(false);
                //Debug.Log("I am NOT Active");
                pauseActive = false;
            }

            else if (pauseActive == false)
            {
                pC.SetActive(true);
                //Debug.Log("I am Active");
                pauseActive = true;
            }

        }
    }

    void Update()
    {
        WakeUpMenu();
    }
}
