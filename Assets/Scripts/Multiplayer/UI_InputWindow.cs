using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    private static UI_InputWindow instance;
    [SerializeField] GameObject nameInputBar;
    bool isHidden = true;

    public void ToggleOnOff()
    {
        if (isHidden) { Show(); }
        else { Hide(); }

        isHidden = !isHidden;
    }

    void Show()
    {
        nameInputBar.SetActive(true);
    }

    void Hide()
    {
        nameInputBar.SetActive(false);
    }
}
