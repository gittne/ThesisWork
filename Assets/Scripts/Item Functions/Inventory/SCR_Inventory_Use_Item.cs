using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_Use_Item : MonoBehaviour
{
    SCR_Inventory_System_Singleplayer inventory;
    [Header("Related Equipment")]
    [SerializeField] SCR_Flashlight_Non_VR flashlight;
    [SerializeField] SCR_FuseBox fuseBox;
    [Header("Item IDs")]
    [SerializeField] string[] itemID;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<SCR_Inventory_System_Singleplayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseBatteries()
    {
        foreach (Inventory_Item item in inventory.inventory)
        {
            if (itemID[0] == item.itemData.itemID && item.stackSize > 0)
            {
                flashlight.RefillBatteries();
                inventory.SubtractItem(item.itemData);
            }
        }
    }

    public void UseFuzes()
    {
        foreach (Inventory_Item item in inventory.inventory)
        {
            if (itemID[1] == item.itemData.itemID && item.stackSize > 0 && fuseBox.canInsertFuse == true)
            {
                fuseBox.FillFusebox();
                inventory.SubtractItem(item.itemData);
            }
        }
    }

    public void UseKeycard()
    {
        foreach (Inventory_Item item in inventory.inventory)
        {
            if (itemID[2] == item.itemData.itemID && item.stackSize > 0)
            {
                Debug.Log("Keycard used");
                inventory.SubtractItem(item.itemData);
            }
        }
    }
}
