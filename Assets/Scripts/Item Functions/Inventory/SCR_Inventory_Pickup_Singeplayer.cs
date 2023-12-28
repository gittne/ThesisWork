using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_Pickup_Singeplayer : MonoBehaviour
{
    [SerializeField] SCR_Inventory_Item_Data referenceItem;
    [SerializeField] GameObject Press_canvas;

    public void OnHandlePickupItem()
    {
        SCR_Inventory_System_Singleplayer.current.AddItem(referenceItem);
        Destroy(gameObject);
        Press_canvas.SetActive(false);
    }
}
