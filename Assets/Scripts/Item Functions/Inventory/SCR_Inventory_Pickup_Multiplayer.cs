using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Inventory_Pickup_Multiplayer : NetworkBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;
    float distance;
    float nearestDistance = 1000000;
    GameObject[] players;
    GameObject nearestPlayer;
    SCR_Inventory_System_Singleplayer nearestInventory;

    private void Awake()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = GameObject.FindWithTag("Player");
        }

        Debug.Log("These are the player in the scene are: " + players);
    }

    private void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, players[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestPlayer = players[i];
                nearestDistance = distance;
            }
        }

        nearestInventory = nearestPlayer.GetComponent<SCR_Inventory_System_Singleplayer>();

        Debug.Log("The nearest player is: " + nearestPlayer + ", with the nearest inventory being: " + nearestInventory);
    }

    public void OnHandlePickupItem()
    {
        nearestInventory.AddItem(referenceItem);
        DestroyPickupServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyPickupServerRPC()
    {
        Destroy(gameObject);
    }
}
