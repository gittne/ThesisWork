using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MultiplayerToggler : MonoBehaviour
{
    bool multiplayerIsEnabled = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneWasChanged;
    }

    public void EnableMultiplayer()
    {
        multiplayerIsEnabled = true;
    }

    void OnSceneWasChanged(Scene scene, LoadSceneMode loadMode)
    {
        if(scene.buildIndex == 1)
        {
            if(!multiplayerIsEnabled) 
            {
                Destroy(GameObject.FindWithTag("MultiplayerCanvas"));
                Destroy(GameObject.FindWithTag("TemporaryMultiDeleteCamera"));
            }
            else
            {
                Destroy(GameObject.FindWithTag("Player"));
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
