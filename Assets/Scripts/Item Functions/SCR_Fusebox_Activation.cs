using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fusebox_Activation : MonoBehaviour
{
    [Header("Fusebox Script")]
    [SerializeField] SCR_FuseBox fusebox;
    [Header("Keycard Script")]
    [SerializeField] SCR_Key_Card_Reader keycardReader;

    // Update is called once per frame
    void Update()
    {
        if (fusebox.isActivated)
        {
            keycardReader.canActivate = true;

        }
        else
        {
            keycardReader.canActivate = false;
        }
    }
}
