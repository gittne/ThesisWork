using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_Use_Item : MonoBehaviour
{
    SCR_Inventory_System_Singleplayer inventory;
    [Header("Related Equipment")]
    [SerializeField] SCR_Flashlight_Non_VR flashlight;
    [SerializeField] SCR_FuseBox[] fuseBoxes;
    [SerializeField] SCR_Key_Card_Reader[] keyReaders;
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
            foreach (SCR_FuseBox fuseBox in fuseBoxes)
            {
                if (fuseBox.canInsertFuse)
                {
                    if (itemID[1] == item.itemData.itemID && item.stackSize > 0 && fuseBox.canInsertFuse == true && fuseBox.isActivated == false)
                    {
                        fuseBox.FillFusebox();
                        inventory.SubtractItem(item.itemData);
                    }
                }
            }
        }
    }

    public void UseKeycard()
    {
        foreach (Inventory_Item item in inventory.inventory)
        {
            foreach (SCR_Key_Card_Reader locks in keyReaders)
            {
                if (locks.canReadCard)
                {
                    if (itemID[2] == item.itemData.itemID && item.stackSize > 0 && locks.canReadCard == true && itemID[2] == locks.keycardItemID)
                    {
                        locks.ReadCard();
                    }
                }
            }
        }
    }
}
