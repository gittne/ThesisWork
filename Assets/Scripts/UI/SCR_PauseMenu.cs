using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SCR_PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject SettingsUI;
    [SerializeField] GameObject ControlsUI;
    [SerializeField] GameObject MainMenuExitScreen;




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        closeAllMenues();
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    void closeAllMenues()
    {
        pauseMenuUI.SetActive(false);
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(false);
        MainMenuExitScreen.SetActive(false);
    }

    // opens menus
    public void ControlMenu()
    {
        ControlsUI.SetActive(true);
    }
    public void SettingsMenu()
    {
      SettingsUI.SetActive(true);
    }

    // closes all menus exept for pausemenu
    public void BackToMenu()
    {
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(false);
        MainMenuExitScreen.SetActive(false);
    }



    // Opens a menu asking for assurance if you want to go to main menu. 
    public void MainMenuAssurance()
    {
        MainMenuExitScreen.SetActive(true);
    }
    public void MainMenu() // loads main menu
    {
      SceneManager.LoadScene(0);
    }


}
