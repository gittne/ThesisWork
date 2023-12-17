using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_Pickup_Singeplayer : MonoBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;

    public void OnHandlePickupItem()
    {
        SCR_Inventory_System_Singleplayer.current.AddItem(referenceItem);
        Debug.Log("Plockade upp föremål");
    }
}
