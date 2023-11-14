using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputWindow : MonoBehaviour
{
    [SerializeField] GameObject nameInputBar;
    bool isHidden = true;

    public void ToggleOnOff()
    {
        if(isHidden) { Show(); }
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
