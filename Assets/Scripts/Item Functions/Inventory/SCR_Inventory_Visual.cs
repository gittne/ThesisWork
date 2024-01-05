using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Inventory_Visual : MonoBehaviour
{
    [Header("Inventory Variables")]
    [SerializeField] KeyCode inventoryKey = KeyCode.Tab;
    [SerializeField] GameObject inventoryPrefab;
    [SerializeField] GameObject backpackPrefab;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    public bool isInventoryActive { get; private set; } = false;

    public void ChangeInventoryState()
    {
        isInventoryActive = !isInventoryActive;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InventoryManagement()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            isInventoryActive = !isInventoryActive;

            if (isInventoryActive)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (isInventoryActive)
        {
            inventoryPrefab.transform.position = Vector3.Lerp(inventoryPrefab.transform.position, endPosition.position, 3f * Time.deltaTime);
        }
        else
        {
            inventoryPrefab.transform.position = Vector3.Lerp(inventoryPrefab.transform.position, startPosition.position, 5f * Time.deltaTime);
        }

        if (inventoryPrefab.transform.position.y <= startPosition.transform.position.y + 0.01f)
        {
            backpackPrefab.SetActive(false);
        }
        else
        {
            backpackPrefab.SetActive(true);
        }
    }
}
