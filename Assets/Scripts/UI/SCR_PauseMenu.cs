using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SCR_PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject SettingsUI;




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        void Pause()
        {
          pauseMenuUI.SetActive(true);
          Time.timeScale = 0f;
          GameIsPaused = true;
        }

    public void SettingsON()
    {
      SettingsUI.SetActive(true);
    }

    public void SettingsOFF()
    {
      SettingsUI.SetActive(false);
    }
    public void MainMenu()
    {
      SceneManager.LoadScene(0);
    }


}
