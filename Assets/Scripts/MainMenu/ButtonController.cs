using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject pausemenu;
    static public string selectedlevel = "Level1";

    public void StartGame()
    {
        SceneManager.LoadScene(selectedlevel);
    }

    public void SelectLevel(string levelname)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelname);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        pausemenu.SetActive(false);
    }
}
