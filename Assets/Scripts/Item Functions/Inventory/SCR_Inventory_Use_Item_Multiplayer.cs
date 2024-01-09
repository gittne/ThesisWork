using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class SCR_Inventory_Use_Item_Multiplayer : NetworkBehaviour
{
    SCR_Inventory_System_Singleplayer inventory;
    SCR_Inventory_Visual_Multiplayer visualInventory;
    [Header("Related Equipment")]
    [SerializeField] SCR_Flashlight_Multiplayer flashlight;
    [SerializeField] SCR_FuseBox[] fuseBoxes;
    [SerializeField] SCR_Key_Card_Reader[] keyReaders;
    [SerializeField] SCR_Squeaky_Toy_Functionality_Multiplayer squeakyToy;
    [Header("Item IDs")]
    [SerializeField] string[] itemID;
    [Header("Item Amount Text")]
    [SerializeField] TextMeshProUGUI[] amountIndicators;
    [Header("Icons")]
    [SerializeField] Image[] itemIcons;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        inventory = GetComponent<SCR_Inventory_System_Singleplayer>();
        visualInventory = GetComponent<SCR_Inventory_Visual_Multiplayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        ShowItemAmountLeft();
        //ShowItemIcons();
    }

    void ShowItemAmountLeft()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[0] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[0].text = item.stackSize.ToString();
            }

            if (itemID[1] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[1].text = item.stackSize.ToString();
            }

            if (itemID[2] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[2].text = item.stackSize.ToString();
            }

            if (itemID[3] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[3].text = item.stackSize.ToString();
            }
        }
    }

    void ShowItemIcons()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        Color alphaColor;

        foreach (Inventory_Item item in inventoryCopy)
        {

        }
    }

    public void UseBatteries()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[0] == item.itemData.itemID && item.stackSize > 0)
            {
                flashlight.RefillBatteries();
                inventory.SubtractItem(item.itemData);
                amountIndicators[0].text = item.stackSize.ToString();
            }
        }
    }

    public void UseFuzes()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            foreach (SCR_FuseBox fuseBox in fuseBoxes)
            {
                if (fuseBox.canInsertFuse)
                {
                    if (itemID[1] == item.itemData.itemID && item.stackSize > 0 && fuseBox.canInsertFuse == true && fuseBox.isActivated == false)
                    {
                        fuseBox.FillFusebox();
                        inventory.SubtractItem(item.itemData);
                        amountIndicators[1].text = item.stackSize.ToString();
                    }
                }
            }
        }
    }

    public void UseKeycard()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
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

    public void UseMrWhiskars()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[3] == item.itemData.itemID && item.stackSize > 0 && !squeakyToy.isHolding)
            {
                squeakyToy.BringUpToy();
                inventory.SubtractItem(item.itemData);
                amountIndicators[3].text = item.stackSize.ToString();
                visualInventory.ChangeInventoryState();
            }
        }
    }
}
