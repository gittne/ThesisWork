using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TempWinScript : MonoBehaviour
{
    [SerializeField] SCR_KeyReader KeyReader;
    [SerializeField] GameObject WinScreen;

    private void Start()
    {
        WinScreen.SetActive(false);
    }

    public void VictoryScreen()
    {
        if(KeyReader.isActivated == true)
        WinScreen.SetActive(true);
    }

}
