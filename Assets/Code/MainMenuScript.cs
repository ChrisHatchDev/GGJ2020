using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;

public class MainMenuScript : MonoBehaviour
{

    public Canvas pauseMenu;
    public GameObject pC;
    public bool pauseActive;

    private Player player;

    void Awake()
    {
        pauseActive = false;

        if(pC) {
            pC.SetActive(false);
        }
        player = ReInput.players.GetPlayer(0);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void WakeUpMenu()
    {
        if (player.GetButtonDown("BackButton"))
        {

            SceneManager.LoadScene(1);
            //Debug.Log("L key was pressed.");

            return;
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
