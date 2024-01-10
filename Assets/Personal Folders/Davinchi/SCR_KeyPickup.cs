using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_KeyPickup : MonoBehaviour
{
    [SerializeField] SCR_KeyReader Keyreader;


    public void Pickup()
    {
        Keyreader.keyPickup();
    }

}
