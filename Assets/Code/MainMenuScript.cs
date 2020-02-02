using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

public void OnPlayButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

public void OnRestartButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

public void OnExitButton()
    {
        Application.Quit();
    }

}
