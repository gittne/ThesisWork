using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Switch_Interactable : MonoBehaviour
{
    private bool isEnabled;
    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    public void Interact()
    {
        if (isEnabled)
            return;

        isEnabled = true;
    }
}
