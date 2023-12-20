using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SCR_Inventory_UI_Manager : MonoBehaviour
{


    public void Start()
    {
        SCR_Inventory_System_Singleplayer.current.inventoryChangedEvent += UpdateInventory;
    }

    void UpdateInventory()
    {
        foreach (Transform transform in transform)
        {
            Destroy(transform.gameObject);
        }
    }
}
