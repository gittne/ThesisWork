using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_Inventory_UI_Slot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] GameObject stackObject;
    [SerializeField] TextMeshProUGUI stackLabel;

    public void SetItem(Inventory_Item item)
    {
        icon.sprite = item.itemData.itemIcon;
        label.text = item.itemData.itemName;

        if (item.stackSize <= 1)
        {
            stackObject.SetActive(false);
            return;
        }
        stackLabel.text = item.stackSize.ToString();
    }
}
