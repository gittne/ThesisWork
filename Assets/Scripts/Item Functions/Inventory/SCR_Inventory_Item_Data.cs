using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class SCR_Inventory_Item_Data : ScriptableObject
{
    [Header("Item ID")]
    [SerializeField] string id;
    public string itemID
    {
        get { return id; }
        private set { id = value; }
    }
    [Header("Item Name")]
    [SerializeField] string displayName;
    public string itemName
    {
        get { return displayName; }
        private set { displayName = value; }
    }
    [Header("Item Icon")]
    [SerializeField] Sprite icon;
    public Sprite itemIcon
    {
        get { return icon; }
        private set { icon = value; }
    }
    [Header("Prefab")]
    [SerializeField] GameObject prefab;
    public GameObject itemPrefab
    {
        get { return prefab; }
        private set { prefab = value; }
    }
}
