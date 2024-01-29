using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Inventory_Pickup_Multiplayer : NetworkBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;
    GameObject[] inventoryObjects;
    SCR_Inventory_System_Multiplayer inventoryToAddTo;
    float nearestDistance = 100000;

    void Awake()
    {
        inventoryObjects = GameObject.FindGameObjectsWithTag("Inventory");
    }

    void Update()
    {
        for (int i = 0; i < inventoryObjects.Length; i++)
        {
            float distance = Vector3.Distance(this.transform.position, inventoryObjects[i].transform.position);

            if (distance < nearestDistance)
            {
                inventoryToAddTo = inventoryObjects[i].GetComponent<SCR_Inventory_System_Multiplayer>();
                nearestDistance = distance;
            }
        }
    }

    public void OnHandlePickupItem()
    {
        inventoryToAddTo.AddItem(referenceItem);
        Destroy(gameObject);
    }
}
