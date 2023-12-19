using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    public SCR_Inventory_Item_Data itemData { get; private set; }
    public int stackSize { get; private set; }

    public Inventory_Item(SCR_Inventory_Item_Data sourceData)
    {
        itemData = sourceData;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void SubtractFromStack()
    {
        stackSize--;
    }
}

public class SCR_Inventory_System_Singleplayer : MonoBehaviour
{
    public static SCR_Inventory_System_Singleplayer current;
    Dictionary<SCR_Inventory_Item_Data, Inventory_Item> itemDictionary;
    public List<Inventory_Item> inventory; //{ get; private set; }

    private void Awake()
    {
        current = this;
        inventory = new List<Inventory_Item>();
        itemDictionary = new Dictionary<SCR_Inventory_Item_Data, Inventory_Item>();
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
