using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fusebox_Activation_VR : MonoBehaviour
{
    [Header("Fusebox Script")]
    [SerializeField] SCR_Fusebox_VR fusebox;
    [Header("Keycard Script")]
    [SerializeField] SCR_Key_Card_Reader[] keycardReader;

    // Update is called once per frame
    void Update()
    {
        if (fusebox.isActivated)
        {
            for (int i = 0; i < keycardReader.Length; i++)
            {
                keycardReader[i].canActivate = true;
            }
        }
        else
        {
            for (int i = 0; i < keycardReader.Length; i++)
            {
                keycardReader[i].canActivate = false;
            }
        }
    }
}
