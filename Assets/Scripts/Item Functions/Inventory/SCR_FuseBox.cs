using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FuseBox : MonoBehaviour
{

    [SerializeField] KeyCode interactionKey = KeyCode.E;
    GameObject obj;

    void OnTriggerEnter(Collider other)
    {
        takeFuse();
    }

    void takeFuse()
    {
        //if (obj.TryGetComponent(out SCR_Inventory_System_Singleplayer inv))
        //{
        //    inv.SubtractItem();
        //}
    }
  
}