using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_System_Multiplayer : MonoBehaviour
{
    public static SCR_Inventory_System_Multiplayer current;
    Dictionary<SCR_Inventory_Item_Data, Inventory_Item> itemDictionary;
    public List<Inventory_Item> inventory;
    public event Action inventoryChangedEvent;

    private void Awake()
    {
        current = this;
        inventory = new List<Inventory_Item>();
        itemDictionary = new Dictionary<SCR_Inventory_Item_Data, Inventory_Item>();
    }

    public void InventoryChanged()
    {
        if (inventoryChangedEvent != null)
        {
            inventoryChangedEvent();
        }
    }

    public Inventory_Item Get(SCR_Inventory_Item_Data itemReferenceData)
    {
        if (itemDictionary.TryGetValue(itemReferenceData, out Inventory_Item value))
        {
            return value;
        }
        return null;
    }

    public void AddItem(SCR_Inventory_Item_Data itemReferenceData)
    {
        if (itemDictionary.TryGetValue(itemReferenceData, out Inventory_Item value))
        {
            value.AddToStack();
        }
        else
        {
            Inventory_Item newItem = new Inventory_Item(itemReferenceData);
            inventory.Add(newItem);
            itemDictionary.Add(itemReferenceData, newItem);
        }
    }

    public void SubtractItem(SCR_Inventory_Item_Data itemReferenceData)
    {
        if (itemDictionary.TryGetValue(itemReferenceData, out Inventory_Item value))
        {
            value.SubtractFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(itemReferenceData);
            }
        }
    }
}
