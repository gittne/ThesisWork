using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MultiplayerToggler : MonoBehaviour
{
    bool multiplayerIsEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
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
        }
    }
}
