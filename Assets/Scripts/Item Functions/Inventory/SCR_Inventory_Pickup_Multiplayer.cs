using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Inventory_Pickup_Multiplayer : NetworkBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;

    [ServerRpc(RequireOwnership = false)]
    public void OnHandlePickupItemServerRPC()
    {
        SCR_Inventory_System_Multiplayer.current.AddItem(referenceItem);
        Destroy(gameObject);
    }
}
