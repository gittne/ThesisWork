using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_Pickup_Multiplayer : MonoBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;

    public void OnHandlePickupItem()
    {
        SCR_Inventory_System_Multiplayer.current.AddItem(referenceItem);
        Destroy(gameObject);
    }
}
