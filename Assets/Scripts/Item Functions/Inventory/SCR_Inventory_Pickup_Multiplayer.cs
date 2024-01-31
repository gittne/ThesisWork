using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Inventory_Pickup_Multiplayer : NetworkBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;

    public void OnHandlePickupItem()
    {
        SCR_Inventory_System_Singleplayer.current.AddItem(referenceItem);
        DestroyPickupServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyPickupServerRPC()
    {
        DestroyPickupClientRPC();
    }

    [ClientRpc]
    void DestroyPickupClientRPC()
    {
        Destroy(gameObject);
    }
}
