using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCR_Menu_Alex : MonoBehaviour
{
    int SceneID;
    [SerializeField] GameObject controlscreen;
    [SerializeField] GameObject Introcut;
    [SerializeField] GameObject Menu;

    private void Awake()
    {
        controlscreen.transform.localScale = new Vector3(0, 0, 0);
        Introcut.transform.localScale = new Vector3(0, 0, 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BlueScene()
    {
        SceneID = 1;
        Introcut.transform.localScale = new Vector3(1, 1, 1);
    } 
    public void RedScene()
    {
        SceneID = 2;
        Introcut.transform.localScale = new Vector3(1, 1, 1);
    }
    public void GreenScene()
    {
        SceneID = 3;
        Introcut.transform.localScale = new Vector3(1,1,1);
    }
    

    public void Controls()
    {
        Menu.transform.localScale = new Vector3(0, 0, 0);
        controlscreen.transform.localScale = new Vector3(1, 1, 1);
    }
    public void MenuLoad()
    {
        Menu.transform.localScale = new Vector3(1, 1, 1);
        controlscreen.transform.localScale = new Vector3(0, 0, 0);
    }

    public void SceneLoader()
    {
        SceneManager.LoadScene(SceneID);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
