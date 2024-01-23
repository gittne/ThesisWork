using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_WinScreen : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;

    private void Awake()
    {
        WinScreen.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            WinScreen.SetActive(true);
        }
    }

    public void MainMenu() // loads main menu
    {
        SceneManager.LoadScene(0);
    }

}
