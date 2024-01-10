using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_KeyReader : MonoBehaviour
{
    [SerializeField] SCR_Inventory_Use_Item InventoryItemUse;

   // [SerializeField] Animator Animation;
    public bool isActivated { get; private set; }
    public bool canReadCard { get; private set; }
    public string keycardItemID;


    void Start()
    {
        isActivated = false;
        canReadCard = false;


    }

    public void keyPickup()
    {
        Debug.Log("key Picked up");
        canReadCard = true;
    }

    public void UnlockDoor()
    {
        Debug.Log("Unlocking door");
        InventoryItemUse.Temp_UseKeycard();
    }

    public void ReadCard()
    {
        if (!isActivated)
        {
            isActivated = true;
        }
    }

}
